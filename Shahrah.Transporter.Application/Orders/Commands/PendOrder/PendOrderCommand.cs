using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.PendOrder;

/// <summary>
/// پند کردن سفارش
/// </summary>
public class PendOrderCommand(int orderId, long personId) : IRequest<Unit>, ITransactionalCommand
{
    public int OrderId { get; } = orderId;
    public long PersonId { get; } = personId;
}