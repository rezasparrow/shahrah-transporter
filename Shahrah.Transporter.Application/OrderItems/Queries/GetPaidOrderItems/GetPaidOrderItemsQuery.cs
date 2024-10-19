using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPaidOrderItems;

public class GetPaidOrderItemsQuery(long personId) : IRequest<IEnumerable<PaidOrderItemDto>>
{
    public long PersonId { get; set; } = personId;
}