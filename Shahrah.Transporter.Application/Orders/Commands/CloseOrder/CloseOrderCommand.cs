using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.CloseOrder;

/// <summary>
/// بستن سفارش
/// </summary>
public class CloseOrderCommand(CloseOrderTypeEnum closeType, int orderId, long? personId) : IRequest<Unit>, ITransactionalCommand
{
    public int OrderId { get; } = orderId;
    public long? PersonId { get; } = personId;
    public CloseOrderTypeEnum CloseType { get; } = closeType;
}

public enum CloseOrderTypeEnum
{
    TryToClose = 1,
    ForceClose = 2,
}