using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Models;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Responses;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.Drivers.Services.Interfaces;
using Shahrah.Transporter.Application.OrderItems.EventPublishers;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.EventPublishers;
using Shahrah.Transporter.Application.Orders.Jobs;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderItemStatus = Shahrah.Transporter.Domain.Enums.OrderItemStatus;
using OrderOptionItem = Shahrah.Transporter.Domain.Entities.OrderOptionItem;

namespace Shahrah.Transporter.Application.Orders.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly AppSettings _appSettings;
    private readonly IPersonService _personService;
    private readonly IDriverService _driverService;
    private readonly OrderItemCreatedEventPublisher _orderItemCreatedEventPublisher;
    private readonly SearchedDriverEventPublisher _searchedDriverEventPublisher;
    private readonly IMessageBus _messageBus;
    private readonly INotificationService _notificationService;
    private readonly IJobScheduler _jobScheduler;
    private readonly OrderRegisteredEventPublisher _orderRegisteredEventPublisher;
    private readonly ICloseOrderService _closeOrderService;
    private readonly IOrderQueryService _orderQueryService;

    public OrderService(IApplicationDbContext dbContext, AppSettings appSettings, IPersonService personService, IDriverService driverService, OrderItemCreatedEventPublisher orderItemCreatedEventPublisher, IMessageBus messageBus, SearchedDriverEventPublisher searchedDriverEventPublisher, INotificationService notificationService, IJobScheduler jobScheduler, OrderRegisteredEventPublisher orderRegisteredEventPublisher, ICloseOrderService closeOrderService, IOrderQueryService orderQueryService)
    {
        _dbContext = dbContext;
        _appSettings = appSettings;
        _personService = personService;
        _driverService = driverService;
        _orderItemCreatedEventPublisher = orderItemCreatedEventPublisher;
        _messageBus = messageBus;
        _searchedDriverEventPublisher = searchedDriverEventPublisher;
        _notificationService = notificationService;
        _jobScheduler = jobScheduler;
        _orderRegisteredEventPublisher = orderRegisteredEventPublisher;
        _closeOrderService = closeOrderService;
        _orderQueryService = orderQueryService;
    }

    public async Task<int> RegisterOrderAsync(RegisterOrderDto orderDto, long personId)
    {
        var order = MapRegisterdOrderToOrderEntity(orderDto, personId);
        await RegisterOrderInDataBase(order);
        await _orderRegisteredEventPublisher.Publish(order.Id, personId);
        return order.Id;
    }

    public async Task AssignDriverToOrder(long personId, int orderId, int vehicleId, decimal price, CancellationToken cancellationToken = default)
    {
        var order = await _orderQueryService.GetOrderWithAllDataAsync(personId, orderId);
        AssignDriverToOrderThrowIfIsNotValid(personId, price, order);

        var allocateDriverResponse = await _driverService.SendMessageToDriverForAllocateDriver(vehicleId, personId, price, order, cancellationToken);

        switch (allocateDriverResponse.Result)
        {
            case AllocateDriverResult.Success:
                await HandleDriverAllocated(order, allocateDriverResponse.Bid, cancellationToken);
                break;

            case AllocateDriverResult.DriverHasActiveOrder:
                throw new DomainException(ErrorMessageResource.DriverHasActiveOrder);

            case AllocateDriverResult.DriversVehicleIsNotMatch:
                throw new DomainException(ErrorMessageResource.DriversVehicleIsNotMatch);

            default:
                throw new DomainException(ErrorMessageResource.DriverAllocationError);
        }
    }

    public async Task FindDriver(long personId, int orderId, decimal minimumPrice, decimal maximumPrice,
        int vehicleRequestedCount, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.SingleAsync(x => x.Id == orderId && x.PersonId == personId, cancellationToken);

        FindDriverThrowIfIsNotValid(minimumPrice, maximumPrice, vehicleRequestedCount, order, personId);

        order.MinimumOfferPrice = minimumPrice;
        order.MaximumOfferPrice = maximumPrice;
        order.VehicleQuantityInSearch = vehicleRequestedCount;
        order.SearchOrPendingOrPricingDeadlineExpiredTime = DateTime.Now.AddSeconds(_appSettings.AuctionDurationBySecond);
        order.Status = OrderStatus.Searching;

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _searchedDriverEventPublisher.Publish(await _orderQueryService.GetOrderWithAllDataAsync(personId, orderId));
        await _notificationService.AddToTransporterPersonGroup(orderId, personId);
    }

    public async Task PendOrder(int orderId, long personId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders
            .Include(x => x.OrderItems)
            .SingleOrDefaultAsync(q => q.Id == orderId && q.PersonId == personId, cancellationToken);

        if (order == null)
            throw new DomainException(ErrorMessageResource.NotFoundError);

        order.Status = OrderStatus.InProgress;
        foreach (var orderItem in order.OrderItems.Where(r => r.Status == OrderItemStatus.DriverNotFound))
        {
            orderItem.Status = OrderItemStatus.PendingForFindDriver;
            orderItem.IsPendItem = true;
        }
        order.SearchOrPendingOrPricingDeadlineExpiredTime = DateTime.Now.AddSeconds(_appSettings.PendingOrderDurationBySecond);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _messageBus.Publish(new OrderProgressedEvent(order.CorrelationId, order.Id, order.SenderRequestId), cancellationToken: cancellationToken);
        await _messageBus.Publish(new OrderPendEvent(order.CorrelationId, order.Id, order.SenderRequestId, _appSettings.PendingOrderDurationBySecond), cancellationToken: cancellationToken);
         _jobScheduler.ScheduleAt<OrderPendingFinishedJob>(builder =>
            builder.WithIdentifier(order.Id.ToString())
                .WithSeconds(_appSettings.PendingOrderDurationBySecond)
                .WithData(new Dictionary<string, string> { { "OrderId", order.Id.ToString() } }));
    }

    public async Task ReSendOrder(long personId, int orderId, long minimumOfferPrice, long maximumOfferPrice, int vehicleQuantity, CancellationToken cancellationToken = default)
    {
        var existOrder = await _dbContext.Orders
            .Include(q => q.Source)
            .Include(q => q.Destination)
            .Include(q => q.OrderOptionItems)
            .SingleOrDefaultAsync(q => q.Id == orderId && q.PersonId == personId, cancellationToken);

        if (existOrder is not { Status: OrderStatus.Closed })
            throw new DomainException(ErrorMessageResource.ResendOrderIsNotPossible);

        if (existOrder.SenderRequestId.HasValue)
            throw new DomainException(ErrorMessageResource.ResendOrderIsNotPossible);

        var order = new RegisterOrderDto
        {
            SourceAddress = new RegisterOrderAddressDto
            {
                CityId = existOrder.Source.CityId,
                Latitude = existOrder.Source.Latitude,
                Longitude = existOrder.Source.Longitude,
            },
            DestinationAddress = new RegisterOrderAddressDto
            {
                CityId = existOrder.Destination.CityId,
                Latitude = existOrder.Destination.Latitude,
                Longitude = existOrder.Destination.Longitude,
            },
            LoadId = existOrder.LoadId,
            LoadDescription = existOrder.LoadDescription,
            LoadingDate = existOrder.LoadingDate,
            PackageId = existOrder.PackageId,
            PackingTypeDescription = existOrder.PackingTypeDescription,
            Weight = existOrder.Weight,
            Value = existOrder.Value,
            Description = existOrder.Description,
            TruckId = existOrder.TruckId,
            VehicleOptionItems = existOrder.OrderOptionItems.Select(r => r.OptionItemId).ToList(),
            IsWeighStationRequire = existOrder.IsWeighStationRequire,
            MinimumOfferPrice = minimumOfferPrice,
            MaximumOfferPrice = maximumOfferPrice,
            VehicleQuantity = vehicleQuantity,
            IsSpecialOffer = existOrder.IsSpecialOffer,
            CorrelationId = Guid.NewGuid()
        };

        await RegisterOrderAsync(order, existOrder.PersonId ?? throw new NullReferenceException("PersonId is null"));
    }

    public async Task ConfirmTransporterOfferPriceBySender(ConfirmTransporterOfferPriceEvent message)
    {
        var order = await _dbContext.Orders.SingleOrDefaultAsync(t => t.SenderRequestId.HasValue && t.SenderRequestId.Value == message.SenderOrderId);
        if (order == null) throw new Exception($"order with SenderOrderId {message.SenderOrderId} not found.");

        if (order.Status != OrderStatus.PriceFinalized && order.Status != OrderStatus.Closed)
        {
            if (message.IsConfitmed)
            {
                order.MaximumOfferPrice = order.TransporterOfferPrice.Value;
                order.Status = OrderStatus.PriceFinalized;
                await _dbContext.SaveChangesAsync();
            }
            else
                await _closeOrderService.Close(CloseOrderTypeEnum.ForceClose, order.Id, order.PersonId ?? throw new NullReferenceException("personid can not be null..."));

            await _notificationService.SendToPerson(order.PersonId.Value, x => x.ConfirmTransporterOfferPrice(order.Id, message.IsConfitmed, order.TransporterOfferPrice.Value));
        }
    }

    public async Task OrderClosedBySender(int orderId)
    {
        var order = await _dbContext.Orders.SingleOrDefaultAsync(x => x.SenderRequestId == orderId);
        if (order == null)
            return;
        await _closeOrderService.Close(CloseOrderTypeEnum.ForceClose, order.Id, order.PersonId);
    }

    private void FindDriverThrowIfIsNotValid(decimal minimumPrice, decimal maximumPrice, int vehicleRequestedCount, Order order, long personId)
    {
        if (order == null)
            throw new DomainException(ErrorMessageResource.OrderProcessByAnotherTc);

        var orderItemsCount = _dbContext.OrderItems
            .Where(q => q.OrderId == order.Id)
            .Count(t => t.Status != OrderItemStatus.Canceled);

        if (orderItemsCount + vehicleRequestedCount > order.VehicleQuantity)
            throw new DomainException(ErrorMessageResource.MaximumVehicleCountAlreadySpecified);

        if (order.VehicleQuantity < vehicleRequestedCount)
            throw new DomainException(ErrorMessageResource.VehicleGreaterThanOrEqualOrderVehicle);

        if (order.SenderUserId.HasValue && order.Status != OrderStatus.PriceFinalized)
            throw new DomainException(ErrorMessageResource.RequestedOperationIsNotPremitedInCurrentSituation);

        if (!order.SenderUserId.HasValue && order.Status != OrderStatus.Registered)
            throw new DomainException(ErrorMessageResource.RequestedOperationIsNotPremitedInCurrentSituation);

        if (!_personService.HasSubscription(personId).Result)
            throw new DomainException(ErrorMessageResource.ActivePlanForTransporterNotFound);

        if (minimumPrice < order.MinimumOfferPrice || order.MaximumOfferPrice < maximumPrice)
            throw new DomainException(ErrorMessageResource.SelectedPriceIsNotInPriceRange);
    }

    private async Task HandleDriverAllocated(Order order, Bid bid, CancellationToken cancellationToken = default)
    {
        var orderItem = new OrderItem
        {
            BidId = bid.Id,
            DriverId = bid.Driver.Id,
            DriverNationalCode = bid.Driver.NationalCode,
            VehicleSmartCardNumber = bid.Driver.Vehicle.SmartCardNumber,
            OfferedPriced = bid.Price,
            Status = OrderItemStatus.WaitingForLoading
        };

        order.Status = OrderStatus.InProgress;
        order.OrderItems ??= new List<OrderItem>();
        order.OrderItems.Add(orderItem);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _orderItemCreatedEventPublisher.Publish(orderItem.Id, bid);
        await _messageBus.Publish(new OrderProgressedEvent(order.CorrelationId, order.Id, order.SenderRequestId), cancellationToken: cancellationToken);
    }

    private void AssignDriverToOrderThrowIfIsNotValid(long personId, decimal price, Order order)
    {
        if (order == null)
            throw new DomainException(ErrorMessageResource.OrderProcessByAnotherTc);

        if (order.OrderItems.Count >= order.VehicleQuantity)
            throw new DomainException(ErrorMessageResource.MaximumVehicleCountAlreadySpecified);

        if (order.Status != OrderStatus.PriceFinalized && order.Status != OrderStatus.InProgress && order.Status != OrderStatus.Searching && (order.SenderRequestId.HasValue || order.Status == OrderStatus.Closed))
            throw new DomainException(ErrorMessageResource.RequestedOperationIsNotPremitedInCurrentSituation);

        if (price > order.MaximumOfferPrice)
            throw new DomainException(ErrorMessageResource.PriceOutOfRangeError);

        if (!_personService.HasSubscription(personId).Result)
            throw new DomainException(ErrorMessageResource.ActivePlanForTransporterNotFound);
    }

    private static Order MapRegisterdOrderToOrderEntity(RegisterOrderDto registerOrder, long personId)
    {
        return new Order
        {
            Source = new Address
            {
                CityId = registerOrder.SourceAddress.CityId,
                Latitude = registerOrder.SourceAddress.Latitude,
                Longitude = registerOrder.SourceAddress.Longitude
            },
            Destination = new Address
            {
                CityId = registerOrder.DestinationAddress.CityId,
                Latitude = registerOrder.DestinationAddress.Latitude,
                Longitude = registerOrder.DestinationAddress.Longitude
            },
            LoadId = registerOrder.LoadId,
            LoadDescription = registerOrder.LoadDescription,
            LoadingDate = registerOrder.LoadingDate,
            PackageId = registerOrder.PackageId,
            PackingTypeDescription = registerOrder.PackingTypeDescription,
            Weight = registerOrder.Weight,
            Value = registerOrder.Value,
            Description = registerOrder.Description,
            TruckId = registerOrder.TruckId,
            OrderOptionItems = registerOrder.VehicleOptionItems?.Select(x => new OrderOptionItem { OptionItemId = x }).ToList(),
            IsWeighStationRequire = registerOrder.IsWeighStationRequire,
            // TODO: Javad Rasouli >> اینا چی هستن؟
            MinimumOfferPrice = registerOrder.MinimumOfferPrice,
            MaximumOfferPrice = registerOrder.MaximumOfferPrice,
            TransporterOfferPrice = registerOrder.MaximumOfferPrice,
            VehicleQuantity = registerOrder.VehicleQuantity,
            IsSpecialOffer = registerOrder.IsSpecialOffer,
            SendingDate = DateTime.Now,
            PersonId = personId,
            CorrelationId = registerOrder.CorrelationId ?? Guid.NewGuid()
        };
    }

    private async Task RegisterOrderInDataBase(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }
}