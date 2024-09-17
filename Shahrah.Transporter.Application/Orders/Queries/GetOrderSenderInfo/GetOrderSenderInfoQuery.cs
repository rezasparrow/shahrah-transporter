using MediatR;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrderSenderInfo;

public class GetOrderSenderInfoQuery : IRequest<OrderSenderInfoDto>
{
    public GetOrderSenderInfoQuery(long personId, int orderId)
    {
        PersonId = personId;
        OrderId = orderId;
    }

    public int OrderId { get; }
    public long PersonId { get; }
}