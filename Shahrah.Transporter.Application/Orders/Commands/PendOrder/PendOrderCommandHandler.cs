using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.PendOrder;

public class PendOrderCommandHandler(IOrderService orderService) : IRequestHandler<PendOrderCommand, Unit>
{
    private readonly IOrderService _orderService = orderService;

    public async Task<Unit> Handle(PendOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderService.PendOrder(request.OrderId, request.PersonId, cancellationToken);

        return Unit.Value;
    }
}