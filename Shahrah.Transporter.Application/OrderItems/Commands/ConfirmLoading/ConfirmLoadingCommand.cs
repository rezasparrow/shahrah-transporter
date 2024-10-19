using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.ConfirmLoading;

/// <summary>
/// تائید بارگیری توسط تی سی
/// این به این معنی نیست که بار حتما بارگیری شده
/// بلکه اعلام یکی از سه نفر میباشد
/// </summary>
public class ConfirmLoadingCommand(int orderItemId, long personId) : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; } = orderItemId;
    public long PersonId { get; } = personId;
}