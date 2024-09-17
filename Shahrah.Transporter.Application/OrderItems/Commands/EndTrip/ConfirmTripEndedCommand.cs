using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.EndTrip;

/// <summary>
/// تائید اتمام سفر توسط تی سی
/// این به این معنی نیست که بار حتما سفر تمام شده
/// بلکه اعلام یکی از سه نفر میباشد
/// </summary>
public class ConfirmTripEndedCommand : IRequest<Unit>, ITransactionalCommand
{
    public ConfirmTripEndedCommand(int orderItemId, long personId)
    {
        OrderItemId = orderItemId;
        PersonId = personId;
    }

    public int OrderItemId { get; }
    public long PersonId { get; }
}