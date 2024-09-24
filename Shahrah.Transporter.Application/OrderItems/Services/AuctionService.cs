using Microsoft.EntityFrameworkCore;
using NetBox.Extensions;
using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.OrderItems.EventPublishers;
using Shahrah.Transporter.Application.OrderItems.Jobs;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderItemStatus = Shahrah.Transporter.Domain.Enums.OrderItemStatus;

namespace Shahrah.Transporter.Application.OrderItems.Services;

public class AuctionService : IAuctionService
{
    private readonly IApplicationDbContext _dbContext;

    private readonly OrderItemChangeStateEventPublisher _orderItemChangeStateEventPublisher;
    private readonly INotificationService _notificationService;
    private readonly IJobScheduler _jobScheduler;
    private readonly OrderItemCreatedEventPublisher _orderItemCreatedEventPublisher;
    private readonly IMessageBus _messageBus;
    private readonly AppSettings _appSettings;
    private readonly ICloseOrderService _closeOrderService;
    private readonly IPersonService _personService;
    private readonly OrderItemPaidEventPublisher _orderItemPaidEventPublisher;

    public AuctionService(IApplicationDbContext dbContext, OrderItemChangeStateEventPublisher orderItemChangeStateEventPublisher, INotificationService notificationService, IJobScheduler jobScheduler, OrderItemCreatedEventPublisher orderItemCreatedEventPublisher, IMessageBus messageBus, AppSettings appSettings, ICloseOrderService closeOrderService, IPersonService personService, OrderItemPaidEventPublisher orderItemPaidEventPublisher)
    {
        _dbContext = dbContext;
        _orderItemChangeStateEventPublisher = orderItemChangeStateEventPublisher;
        _notificationService = notificationService;
        _jobScheduler = jobScheduler;
        _orderItemCreatedEventPublisher = orderItemCreatedEventPublisher;
        _messageBus = messageBus;
        _appSettings = appSettings;
        _closeOrderService = closeOrderService;
        _personService = personService;
        _orderItemPaidEventPublisher = orderItemPaidEventPublisher;
    }

    public async Task AuctionClosed(AuctionClosedEvent message)
    {
        var order = await _dbContext.Orders
            .Include(x => x.OrderItems)
            .SingleAsync(q => q.Id == message.OrderId);

        if (!order.PersonId.HasValue)
            throw new NullReferenceException("Order PersonId can not be null...");

        if (order.Status is not OrderStatus.Searching and not OrderStatus.InProgress)
            return;

        switch (message.Reason)
        {
            case AuctionCloseReasonEnum.DriverNotFound:
                await HandleDriverNotFound(order);
                return;

            case AuctionCloseReasonEnum.DriverNotRespond:
                await HandleDriverNotRespond(order, message.CauseToSuspendTransporterPerson);
                return;

            case AuctionCloseReasonEnum.AnnounceWinnerDriver:
                await HandleDriverFound(order, message.WinningBids, message.AuctionWasPending);
                return;

            case AuctionCloseReasonEnum.PendingAuctionTimeout:
                await HandlePendingAuctionTimeout(order);
                return;

            case AuctionCloseReasonEnum.ForceClose:
                if (order.Status is not OrderStatus.Closed)
                    await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, order.Id, order.PersonId ?? throw new NullReferenceException("personid can not be null..."));
                return;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task HandlePendingAuctionTimeout(Order order)
    {
        var itemsPendingForFindDriver = order.OrderItems.Where(r => r.IsPendItem && (r.Status is OrderItemStatus.PendingForFindDriver or OrderItemStatus.PendingPayment or OrderItemStatus.AttemptToPay)).ToList();
        foreach (var orderItem in itemsPendingForFindDriver)
        {
            orderItem.Status = OrderItemStatus.Canceled;
            _dbContext.OrderItems.Update(orderItem);
            RemoveOrderItemJobs(orderItem);
            await _orderItemChangeStateEventPublisher.Publish(orderItem.Id, null, OrderItemChangeStateReasonEnum.PendingAuctionTimeout);
        }

        await _dbContext.SaveChangesAsync();

        if (itemsPendingForFindDriver.Any())
            await _notificationService.SendToPerson(order.PersonId.Value, x => x.OrderPendingFinished(order.Id));

        await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, order.Id, order.PersonId ?? throw new NullReferenceException("personid can not be null..."));
    }

    private void RemoveOrderItemJobs(OrderItem orderItem)
    {
         _jobScheduler.Remove<OrderItemAttemptToPayExpiredJob>(orderItem.Id.ToString());
         _jobScheduler.Remove<OrderItemPendingPaymentExpiredJob>(orderItem.Id.ToString());
    }

    private async Task HandleDriverNotFound(Order order)
    {
        order.Status = OrderStatus.InProgress;
        order.SearchOrPendingOrPricingDeadlineExpiredTime = null;
        order.OrderItems ??= new List<OrderItem>();
        order.OrderItems.AddRange(Enumerable.Repeat(0, order.VehicleQuantityInSearch).Select(_ => new OrderItem
        {
            OrderId = order.Id,
            Status = OrderItemStatus.DriverNotFound,
        }));
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();

        foreach (var orderItem in order.OrderItems)
            await _orderItemCreatedEventPublisher.Publish(orderItem.Id, null);

        await _messageBus.Publish(new OrderProgressedEvent(order.CorrelationId, order.Id, order.SenderRequestId));

        await _notificationService.SendToPerson(order.PersonId ?? throw new NullReferenceException("personid can not be null..."), x => x.DriverNotFound(order.Id));

        if (order.VehicleQuantityInSearch == order.VehicleQuantity)
            await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, order.Id, order.PersonId ?? throw new NullReferenceException("personid can not be null..."));
    }

    private async Task HandleDriverNotRespond(Order order, bool causeToSuspendTransporterPerson)
    {
        order.Status = OrderStatus.InProgress;
        order.SearchOrPendingOrPricingDeadlineExpiredTime = null;
        order.OrderItems ??= new List<OrderItem>();
        order.OrderItems.AddRange(Enumerable.Repeat(0, order.VehicleQuantityInSearch).Select(_ => new OrderItem
        {
            OrderId = order.Id,
            Status = OrderItemStatus.DriverNotRespond,
        }));
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();

        foreach (var orderItem in order.OrderItems)
            await _orderItemCreatedEventPublisher.Publish(orderItem.Id, null);

        await _messageBus.Publish(new OrderProgressedEvent(order.CorrelationId, order.Id, order.SenderRequestId));

        await _notificationService.SendToPerson(order.PersonId ?? throw new NullReferenceException("personid can not be null..."), x => x.DriverNotRespond(order.Id, causeToSuspendTransporterPerson));
        if (causeToSuspendTransporterPerson)
            await _personService.Suspend(order.PersonId.Value);

        if (order.VehicleQuantityInSearch == order.VehicleQuantity)
            await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, order.Id, order.PersonId ?? throw new NullReferenceException("personid can not be null..."));
    }

    private async Task HandleDriverFound(Order order, List<Bid> winningBids, bool auctionWasPending)
    {
        if (auctionWasPending)
            await HandleDriverFoundForPendingItem(order, winningBids);
        else
            await HandleDriverFoundForNotPendingItem(order, winningBids);
    }

    private async Task HandleDriverFoundForNotPendingItem(Order order, List<Bid> winningBids)
    {
        order.Status = OrderStatus.InProgress;
        order.SearchOrPendingOrPricingDeadlineExpiredTime = null;
        order.OrderItems ??= new List<OrderItem>();

        var remainderItem = order.VehicleQuantityInSearch - winningBids.Count;
        if (remainderItem > 0)
            order.OrderItems.AddRange(Enumerable.Repeat(0, remainderItem).Select(_ => new OrderItem
            {
                OrderId = order.Id,
                Status = OrderItemStatus.DriverNotRespond,
            }));

        order.OrderItems.AddRange(winningBids.Select(bid => new OrderItem
        {
            OrderId = order.Id,
            BidId = bid.Id,
            DriverId = bid.Driver.Id,
            OfferedPriced = bid.Price,
            Status = _appSettings.IsAppFree ? OrderItemStatus.WaitingForLoading : OrderItemStatus.PendingPayment,
            DriverNationalCode = bid.Driver.NationalCode,
            VehicleSmartCardNumber = bid.Driver.Vehicle.SmartCardNumber,
            PaymentDeadlineExpiredTime = _appSettings.IsAppFree ? null : DateTime.Now.AddSeconds(_appSettings.PaymentDuration)
        }));

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();

        await _messageBus.Publish(new OrderProgressedEvent(order.CorrelationId, order.Id, order.SenderRequestId));

        foreach (var orderItem in order.OrderItems)
        {
            if (orderItem.Status == OrderItemStatus.PendingPayment)
                 _jobScheduler.ScheduleAt<OrderItemPendingPaymentExpiredJob>(builder =>
                                                                        builder.WithIdentifier(orderItem.Id.ToString())
                                                                        .WithSeconds(_appSettings.PaymentDuration)
                                                                        .WithData(new Dictionary<string, string> { { "OrderItemId", orderItem.Id.ToString() } }));

            await _orderItemCreatedEventPublisher.Publish(orderItem.Id, winningBids.SingleOrDefault(r => r.Id == orderItem.BidId));

            if (_appSettings.IsAppFree)
                await _orderItemPaidEventPublisher.Publish(orderItem.Id);
        }

        await _notificationService.SendToPerson(order.PersonId ?? throw new NullReferenceException("personid can not be null..."), x => x.DriverAccept(order.Id));
    }

    private async Task HandleDriverFoundForPendingItem(Order order, List<Bid> winningBids)
    {
        var orderItems = order.OrderItems.Where(r => r.Status == OrderItemStatus.PendingForFindDriver)
            .OrderByDescending(o => o.Id)
            .Take(winningBids.Count)
            .ToList();

        if (!orderItems.Any())
            return;

        var index = 0;
        foreach (var bid in winningBids)
        {
            orderItems[index].OrderId = order.Id;
            orderItems[index].BidId = bid.Id;
            orderItems[index].DriverId = bid.Driver.Id;
            orderItems[index].OfferedPriced = bid.Price;
            orderItems[index].Status = _appSettings.IsAppFree ? OrderItemStatus.WaitingForLoading : OrderItemStatus.PendingPayment;
            orderItems[index].DriverNationalCode = bid.Driver.NationalCode;
            orderItems[index].VehicleSmartCardNumber = bid.Driver.Vehicle.SmartCardNumber;
            orderItems[index].PaymentDeadlineExpiredTime = _appSettings.IsAppFree ? null : DateTime.Now.AddSeconds(_appSettings.PaymentDuration);
            index++;
        }

        if (order.OrderItems.All(r => r.Status != OrderItemStatus.PendingForFindDriver))
            order.SearchOrPendingOrPricingDeadlineExpiredTime = null;

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();

        foreach (var orderItem in orderItems)
        {
            if (!_appSettings.IsAppFree)
                _jobScheduler.ScheduleAt<OrderItemPendingPaymentExpiredJob>(builder =>
                                                                        builder.WithIdentifier(orderItem.Id.ToString())
                                                                        .WithSeconds(_appSettings.PaymentDuration)
                                                                        .WithData(new Dictionary<string, string> { { "OrderItemId", orderItem.Id.ToString() } }));

            await _orderItemChangeStateEventPublisher.Publish(orderItem.Id, winningBids.Single(r => r.Id == orderItem.BidId), OrderItemChangeStateReasonEnum.DriverFoundForPendingItem);

            if (_appSettings.IsAppFree)
                await _orderItemPaidEventPublisher.Publish(orderItem.Id);
        }

        await _notificationService.SendToPerson(order.PersonId ?? throw new NullReferenceException("personid can not be null..."), x => x.DriverAccept(order.Id));
    }
}