using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventPublishers;

public class OrderItemChangeStateEventPublisher
{
    private readonly IMessageBus _messageBus;
    private readonly IApplicationDbContext _dbContext;

    public OrderItemChangeStateEventPublisher(IMessageBus messageBus, IApplicationDbContext dbContext)
    {
        _messageBus = messageBus;
        _dbContext = dbContext;
    }

    public async Task Publish(int orderItemId, Bid bid, OrderItemChangeStateReasonEnum reason)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId);

        if (orderItem == null)
            return;

        await _messageBus.Publish(new OrderItemChangeStateEvent(orderItem.Order.CorrelationId, (OrderItemStatus)orderItem.Status, reason)
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