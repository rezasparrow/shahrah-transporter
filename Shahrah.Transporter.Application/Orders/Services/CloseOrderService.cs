using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.Jobs;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Services;

public class CloseOrderService : ICloseOrderService
{
    private readonly IMessageBus _messageBus;
    private readonly INotificationService _notificationService;
    private readonly IJobScheduler _jobScheduler;
    private readonly IApplicationDbContext _dbContext;

    public CloseOrderService(IMessageBus messageBus, INotificationService notificationService, IJobScheduler jobScheduler, IApplicationDbContext dbContext)
    {
        _messageBus = messageBus;
        _notificationService = notificationService;
        _jobScheduler = jobScheduler;
        _dbContext = dbContext;
    }

    public async Task Close(CloseOrderTypeEnum closeType, int orderId, long? personId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders
            .Include(x => x.OrderItems)
            .Include(x => x.Receivers)
            .SingleOrDefaultAsync(q => q.Id == orderId, cancellationToken);

        if (order == null || order.PersonId != personId)
            throw new DomainException(string.Format(ErrorMessageResource.NotFoundError, orderId));

        switch (closeType)
        {
            case CloseOrderTypeEnum.ForceClose:
                await ForceCloseOrder(order);
                break;

            case CloseOrderTypeEnum.TryToClose:
                await TryToCloseOrder(order);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(closeType), closeType, null);
        }
    }

    private async Task TryToCloseOrder(Order order)
    {
        var completeOrderItemsCount = order.OrderItems.Count(q => q.Status is OrderItemStatus.Canceled or OrderItemStatus.DriverNotFound or OrderItemStatus.DriverNotRespond or OrderItemStatus.TripEnded);
        if (completeOrderItemsCount == order.VehicleQuantity && order.Status != OrderStatus.Closed)
            await CloseOrder(order);
    }

    private async Task ForceCloseOrder(Order order)
    {
        var orderIsClosable = order.OrderItems.All(q => q.Status is OrderItemStatus.Canceled or OrderItemStatus.DriverNotFound or OrderItemStatus.DriverNotRespond or OrderItemStatus.TripEnded) && order.Status != OrderStatus.Closed;
        if (orderIsClosable)
            await CloseOrder(order);
    }

    private async Task CloseOrder(Order order)
    {
        order.Status = OrderStatus.Closed;
        order.SearchOrPendingOrPricingDeadlineExpiredTime = null;
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
        await NoticeReceiversOrderClosed(order);
        await _messageBus.Publish(new OrderClosedEvent(order.CorrelationId, order.Id, order.SenderRequestId));
        await RemoveOrderJobs(order.Id);
    }

    private async Task NoticeReceiversOrderClosed(Order order)
    {
        if (order.PersonId.HasValue)
            await _notificationService.SendToPerson(order.PersonId.Value, x => x.OrderClosed(order.Id));
        else
            foreach (var receiver in order.Receivers)
                await _notificationService.SendToPerson(receiver.PersonId, x => x.OrderClosed(order.Id));
    }

    private async Task RemoveOrderJobs(int orderId)
    {
         _jobScheduler.Remove<OrderPendingFinishedJob>(orderId.ToString());
         _jobScheduler.Remove<OrderPricingFinishedJob>(orderId.ToString());
    }
}