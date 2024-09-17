using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPaidOrderItems;

public class GetPaidOrderItemsQuery : IRequest<IEnumerable<PaidOrderItemDto>>
{
    public GetPaidOrderItemsQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; set; }
}