using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.RegisterOrder;

internal class RegisterOrderCommandHandler(IOrderService orderService) : IRequestHandler<RegisterOrderCommand, Unit>
{
    private readonly IOrderService _orderService = orderService;

    public async Task<Unit> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = await _orderService.RegisterOrderAsync(request.Order, request.PersonId);
        return Unit.Value;
    }
}