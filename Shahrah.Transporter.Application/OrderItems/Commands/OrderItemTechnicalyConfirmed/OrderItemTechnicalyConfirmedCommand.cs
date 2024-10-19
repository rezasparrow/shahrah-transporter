using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.OrderItemTechnicalyConfirmed;

/// <summary>
/// تائید فنی توسط تی سی
/// </summary>
public class OrderItemTechnicalyConfirmedCommand(int orderItemId, long personId) : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; } = orderItemId;
    public long PersonId { get; } = personId;
}