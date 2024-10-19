using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetCanceledOrderItems;

public class GetCanceledOrderItemsQuery(long personId) : IRequest<IEnumerable<CanceledOrderItemDto>>
{
    public long PersonId { get; set; } = personId;
}