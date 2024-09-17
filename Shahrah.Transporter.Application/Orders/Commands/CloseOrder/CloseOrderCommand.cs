using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.CloseOrder;

/// <summary>
/// بستن سفارش
/// </summary>
public class CloseOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public CloseOrderCommand(CloseOrderTypeEnum closeType, int orderId, long? personId)
    {
        OrderId = orderId;
        PersonId = personId;
        CloseType = closeType;
    }

    public int OrderId { get; }
    public long? PersonId { get; }
    public CloseOrderTypeEnum CloseType { get; }
}

public enum CloseOrderTypeEnum
{
    TryToClose = 1,
    ForceClose = 2,
}