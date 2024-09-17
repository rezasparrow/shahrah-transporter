using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.PendOrder;

/// <summary>
/// پند کردن سفارش
/// </summary>
public class PendOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public PendOrderCommand(int orderId, long personId)
    {
        OrderId = orderId;
        PersonId = personId;
    }

    public int OrderId { get; }
    public long PersonId { get; }
}