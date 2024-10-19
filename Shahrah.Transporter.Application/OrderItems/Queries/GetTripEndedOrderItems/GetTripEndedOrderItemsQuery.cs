using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetTripEndedOrderItems;

public class GetTripEndedOrderItemsQuery(long personId) : IRequest<IEnumerable<TripEndedOrderItemDto>>
{
    public long PersonId { get; } = personId;
}