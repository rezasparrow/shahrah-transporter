using MediatR;
using Shahrah.Transporter.Application.Orders.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    public GetOrdersQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}