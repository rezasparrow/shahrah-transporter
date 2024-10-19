using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.OrderItems.EventPublishers;

public class OrderItemPaidEventPublisher(IMessageBus messageBus, IApplicationDbContext dbContext)
{
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task Publish(int orderItemId)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .ThenInclude(order => order.Person)
            .ThenInclude(person => person.Transporter)
            .SingleOrDefaultAsync(x => x.Id == orderItemId);

        if (orderItem == null)
            return;

        await _messageBus.Publish(new OrderItemPaidEvent(orderItem.Order.CorrelationId)
        {
            OrderItemId = orderItem.Id,
            OrderItemStatus = Framework.Models.OrderItemStatus.WaitingForLoading,
            SenderOrderId = orderItem.Order.SenderRequestId,
            BidId = orderItem.BidId,
            TransporterOrderItemId = orderItem.Id,
            PaidAmount = orderItem.PaidAmount ?? 0,
            PaymentDate = orderItem.PaymentDate ?? System.DateTime.Now,
            SuggesterTransporterName = orderItem.Order.Person.Transporter.Name,
            SuggesterTransporterPhoneNumber = orderItem.Order.Person.Transporter.PhoneNumber,
            PayerTransporterAgentFirstName = orderItem.Order.Person.FirstName,
            PayerTransporterAgentLastName = orderItem.Order.Person.LastName,
            PayerTransporterAgentMobileNumber = orderItem.Order.Person.MobileNumber
        });
    }
}