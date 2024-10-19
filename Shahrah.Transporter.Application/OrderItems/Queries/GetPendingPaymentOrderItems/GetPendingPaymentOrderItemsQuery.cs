using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPendingPaymentOrderItems;

public class GetPendingPaymentOrderItemsQuery(long personId) : IRequest<IEnumerable<OrderItemDto>>
{
    public long PersonId { get; set; } = personId;
}