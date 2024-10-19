using MediatR;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrder;

public class GetOrderQuery(long personId, int orderId) : IRequest<OrderDto>
{
    public int OrderId { get; } = orderId;
    public long PersonId { get; } = personId;
}