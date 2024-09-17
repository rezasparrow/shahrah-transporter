using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.HideOrder;

public class HideOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public HideOrderCommand(int orderId, long personId)
    {
        OrderId = orderId;
        PersonId = personId;
    }

    public int OrderId { get; }
    public long PersonId { get; }
}