using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetOrderItems;

public class GetOrderItemsQuery(int orderId, long personId) : IRequest<IEnumerable<OrderItemDto>>
{
    public int OrderId { get; } = orderId;
    public long PersonId { get; } = personId;
}