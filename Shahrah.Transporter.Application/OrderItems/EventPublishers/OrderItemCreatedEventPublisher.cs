using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.OrderItems.EventPublishers;

public class OrderItemCreatedEventPublisher(IMessageBus messageBus, IApplicationDbContext dbContext)
{
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task Publish(int orderItemId, Bid bid)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId);

        if (orderItem == null)
            return;

        await _messageBus.Publish(new OrderItemCreatedEvent(orderItem.Order.CorrelationId, (OrderItemStatus)orderItem.Status)
        {
            SenderOrderId = orderItem.Order.SenderRequestId,
            TransporterOrderItemId = orderItem.Id,
            Amount = orderItem.OfferedPriced,

            OrderId = orderItem.OrderId,
            Bid = bid,
            OfferedPriced = orderItem.OfferedPriced,
            PaymentDeadlineExpiredTime = orderItem.PaymentDeadlineExpiredTime
        });
    }
}