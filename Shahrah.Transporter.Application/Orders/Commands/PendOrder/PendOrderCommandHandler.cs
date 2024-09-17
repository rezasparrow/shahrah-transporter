using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Commands.PendOrder;

public class PendOrderCommandHandler : IRequestHandler<PendOrderCommand, Unit>
{
    private readonly IOrderService _orderService;

    public PendOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(PendOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderService.PendOrder(request.OrderId, request.PersonId, cancellationToken);

        return Unit.Value;
    }
}