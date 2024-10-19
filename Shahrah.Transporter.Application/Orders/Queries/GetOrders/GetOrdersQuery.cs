using MediatR;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrders;

public class GetOrdersQuery(long personId) : IRequest<IEnumerable<OrderDto>>
{
    public long PersonId { get; } = personId;
}