using MediatR;
using Shahrah.Transporter.Application.OrderItems.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPendingPaymentOrderItems;

public class GetPendingPaymentOrderItemsQuery : IRequest<IEnumerable<OrderItemDto>>
{
    public GetPendingPaymentOrderItemsQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; set; }
}