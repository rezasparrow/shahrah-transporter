using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.OrderItemTechnicalyConfirmed;

/// <summary>
/// تائید فنی توسط تی سی
/// </summary>
public class OrderItemTechnicalyConfirmedCommand : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; }
    public long PersonId { get; }

    public OrderItemTechnicalyConfirmedCommand(int orderItemId, long personId)
    {
        OrderItemId = orderItemId;
        PersonId = personId;
    }
}