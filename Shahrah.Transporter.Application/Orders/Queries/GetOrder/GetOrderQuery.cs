using MediatR;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrder;

public class GetOrderQuery : IRequest<OrderDto>
{
    public GetOrderQuery(long personId, int orderId)
    {
        PersonId = personId;
        OrderId = orderId;
    }

    public int OrderId { get; }
    public long PersonId { get; }
}