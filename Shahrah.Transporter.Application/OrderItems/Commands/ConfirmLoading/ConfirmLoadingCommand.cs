using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.ConfirmLoading;

/// <summary>
/// تائید بارگیری توسط تی سی
/// این به این معنی نیست که بار حتما بارگیری شده
/// بلکه اعلام یکی از سه نفر میباشد
/// </summary>
public class ConfirmLoadingCommand : IRequest<Unit>, ITransactionalCommand
{
    public ConfirmLoadingCommand(int orderItemId, long personId)
    {
        OrderItemId = orderItemId;
        PersonId = personId;
    }

    public int OrderItemId { get; }
    public long PersonId { get; }
}