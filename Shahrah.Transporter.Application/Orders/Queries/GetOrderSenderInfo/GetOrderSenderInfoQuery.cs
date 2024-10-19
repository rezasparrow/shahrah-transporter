using MediatR;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrderSenderInfo;

public class GetOrderSenderInfoQuery(long personId, int orderId) : IRequest<OrderSenderInfoDto>
{
    public int OrderId { get; } = orderId;
    public long PersonId { get; } = personId;
}