using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.EndTrip;

/// <summary>
/// تائید اتمام سفر توسط تی سی
/// این به این معنی نیست که بار حتما سفر تمام شده
/// بلکه اعلام یکی از سه نفر میباشد
/// </summary>
public class ConfirmTripEndedCommand(int orderItemId, long personId) : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; } = orderItemId;
    public long PersonId { get; } = personId;
}