using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetCanceledOrderItems;

public class GetCanceledOrderItemsQuery : IRequest<IEnumerable<CanceledOrderItemDto>>
{
    public GetCanceledOrderItemsQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; set; }
}