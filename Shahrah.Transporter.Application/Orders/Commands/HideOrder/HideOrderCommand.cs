using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.HideOrder;

public class HideOrderCommand(int orderId, long personId) : IRequest<Unit>, ITransactionalCommand
{
    public int OrderId { get; } = orderId;
    public long PersonId { get; } = personId;
}