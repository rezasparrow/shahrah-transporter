using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.EventPublishers;
using Shahrah.Transporter.Application.Orders.Jobs;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Application.Transporters.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Services;

public class HandleRegisteredOrderBySenderService : IHandleRegisteredOrderBySenderService
{
    private readonly INotificationService _notificationService;
    private readonly TransporterRegisteredSenderOrderEventPublisher _transporterRegisteredSenderOrderEventPublisher;
    private readonly IApplicationDbContext _dbContext;
    private readonly IJobScheduler _jobScheduler;
    private readonly ITransporterService _transporterService;

    public HandleRegisteredOrderBySenderService(INotificationService notificationService, TransporterRegisteredSenderOrderEventPublisher transporterRegisteredSenderOrderEventPublisher, IApplicationDbContext dbContext, IJobScheduler jobScheduler, ITransporterService transporterService)
    {
        _notificationService = notificationService;
        _transporterRegisteredSenderOrderEventPublisher = transporterRegisteredSenderOrderEventPublisher;
        _dbContext = dbContext;
        _jobScheduler = jobScheduler;
        _transporterService = transporterService;
    }

    public async Task Handle(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent, CancellationToken cancellationToken = default)
    {
        var order = MapRegisteredOrderBySenderEventToOrderEntity(orderRegisteredBySenderEvent);
        await SpecifyOrderRecivers(orderRegisteredBySenderEvent, order);
        await RegisterOrderInDataBase(order);
        await NotifyRecivers(order);
        ScheduleOrderPricingJob(order);

        await _transporterRegisteredSenderOrderEventPublisher.Publish(order.Id, order.PersonId);
    }

    private void ScheduleOrderPricingJob(Order order)
    {
        if (order.SearchOrPendingOrPricingDeadlineExpiredTime.HasValue)
             _jobScheduler.ScheduleAt<OrderPricingFinishedJob>(builder =>
                builder.WithIdentifier(order.Id.ToString())
                    .WithSeconds((int)(order.SearchOrPendingOrPricingDeadlineExpiredTime.Value - DateTime.Now).TotalSeconds)
                    .WithData(new Dictionary<string, string> { { "OrderId", order.Id.ToString() } }));
    }

    private async Task RegisterOrderInDataBase(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    private async Task NotifyRecivers(Order order)
    {
        if (order.PersonId.HasValue)
        {
            await _notificationService.AddToTransporterPersonGroup(order.Id, order.PersonId.Value);
            await _notificationService.SendToPerson(order.PersonId.Value, x => x.OrderArrived(order.Id));
        }

        foreach (var item in order.Receivers ?? Enumerable.Empty<PersonOrder>())
            await _notificationService.AddToTransporterPersonGroup(order.Id, item.PersonId);

        await _notificationService.SendToTransporterPersonGroup(order.Id, x => x.OrderArrived(order.Id));
    }

    private static Order MapRegisteredOrderBySenderEventToOrderEntity(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent)
    {
        var order = new Order
        {
            Source = new Address
            {
                CityId = orderRegisteredBySenderEvent.Source.CityId,
                Latitude = orderRegisteredBySenderEvent.Source.Latitude,
                Longitude = orderRegisteredBySenderEvent.Source.Longitude
            },
            Destination = new Address
            {
                CityId = orderRegisteredBySenderEvent.Destination.CityId,
                Latitude = orderRegisteredBySenderEvent.Destination.Latitude,
                Longitude = orderRegisteredBySenderEvent.Destination.Longitude
            },
            LoadId = orderRegisteredBySenderEvent.LoadId,
            LoadDescription = orderRegisteredBySenderEvent.LoadDescription,
            LoadingDate = orderRegisteredBySenderEvent.LoadingDate,
            PackageId = orderRegisteredBySenderEvent.PackageId,
            PackingTypeDescription = orderRegisteredBySenderEvent.PackageDescription,
            Weight = orderRegisteredBySenderEvent.Weight,
            Value = orderRegisteredBySenderEvent.Value,
            Description = orderRegisteredBySenderEvent.Description,
            TruckId = orderRegisteredBySenderEvent.TruckId,
            OrderOptionItems = orderRegisteredBySenderEvent.OrderOptionItems?.Select(x => new OrderOptionItem { OptionItemId = x.OptionItemId }).ToList(),
            IsWeighStationRequire = orderRegisteredBySenderEvent.IsWeighStationRequire,
            // TODO: Javad Rasouli >> اینا چی هستن؟
            MinimumOfferPrice = orderRegisteredBySenderEvent.MinimumOfferPrice,
            MaximumOfferPrice = orderRegisteredBySenderEvent.MaximumOfferPrice,
            SenderOfferPrice = orderRegisteredBySenderEvent.MaximumOfferPrice,
            VehicleQuantity = orderRegisteredBySenderEvent.VehicleQuantity,
            SendingDate = DateTime.Now,
            SenderRequestId = orderRegisteredBySenderEvent.Id,
            SenderName = orderRegisteredBySenderEvent.SenderName,
            SenderMobileNumber = orderRegisteredBySenderEvent.SenderMobileNumber,
            SenderUserId = orderRegisteredBySenderEvent.SenderUserId,
            CorrelationId = orderRegisteredBySenderEvent.CorrelationId,
            Status = OrderStatus.Registered,
            SearchOrPendingOrPricingDeadlineExpiredTime = orderRegisteredBySenderEvent.MaxTimeToResponseBySecond == -1 ? null : DateTime.Now.AddSeconds(orderRegisteredBySenderEvent.MaxTimeToResponseBySecond)
        };

        return order;
    }

    private async Task SpecifyOrderRecivers(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent, Order order)
    {
        // ارسال کننده ایجنت را انتخاب کرده است
        if (HasSenderSpecifiedPerson(orderRegisteredBySenderEvent.PersonId))
        {
            order.PersonId = orderRegisteredBySenderEvent.PersonId;
            return;
        }

        // ارسال کننده باربری را مشخص کرده است
        if (HasSenderSpecifiedTransporter(orderRegisteredBySenderEvent.TransporterId))
        {
            var transporterAgents = await _dbContext.People.Where(t =>
                    t.TransporterId == orderRegisteredBySenderEvent.TransporterId.Value &&
                    t.Status == PersonStatus.Active)
                .ToListAsync();
            order.Receivers = transporterAgents.Select(r => new PersonOrder { PersonId = r.Id }).ToList();
            return;
        }

        // ارسال کننده باربری را مشخص نکرده است
        var people = await _transporterService.GetActivePersonsByTransportersLatLong(orderRegisteredBySenderEvent.Source.Latitude, orderRegisteredBySenderEvent.Source.Longitude);
        order.Receivers = people.Select(person => new PersonOrder { PersonId = person.Id }).ToList();
    }

    private static bool HasSenderSpecifiedTransporter(int? transporterId)
    {
        return transporterId.HasValue;
    }

    private static bool HasSenderSpecifiedPerson(long? personId)
    {
        return personId.HasValue;
    }
}