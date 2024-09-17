using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetTripEndedOrderItems;

public class GetTripEndedOrderItemsQuery : IRequest<IEnumerable<TripEndedOrderItemDto>>
{
    public GetTripEndedOrderItemsQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}