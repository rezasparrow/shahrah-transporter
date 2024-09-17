using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetOrderItems;

public class GetOrderItemsQuery : IRequest<IEnumerable<OrderItemDto>>
{
    public GetOrderItemsQuery(int orderId, long personId)
    {
        OrderId = orderId;
        PersonId = personId;
    }

    public int OrderId { get; }
    public long PersonId { get; }
}