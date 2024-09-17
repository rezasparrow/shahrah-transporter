using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForTripEnd;

/// <summary>
/// بررسی اینکه اگر دونفر لز سه نفر تائید اتمام سفر رو زده باشن، وضعیت لاین آیتم تغییر کنه
/// </summary>
public class CheckOrderItemForTripEndCommand : IRequest<Unit>, ITransactionalCommand
{
    public CheckOrderItemForTripEndCommand(int orderItemId)
    {
        OrderItemId = orderItemId;
    }

    public int OrderItemId { get; }
}