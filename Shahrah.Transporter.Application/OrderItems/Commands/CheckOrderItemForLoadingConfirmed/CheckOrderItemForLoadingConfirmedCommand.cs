using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForLoadingConfirmed;

/// <summary>
/// بررسی اینکه اگر دو نفر از سه نفر تائید بارگیری زده اند وضعیت لاین آیتم عوض شود
/// </summary>
public class CheckOrderItemForLoadingConfirmedCommand(int orderItemId) : IRequest<Unit>, ITransactionalCommand
{
    public int OrderItemId { get; } = orderItemId;
}