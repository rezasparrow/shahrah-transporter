using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandler(ICloseOrderService closeOrderService) : IRequestHandler<CloseOrderCommand, Unit>
{
    private readonly ICloseOrderService _closeOrderService = closeOrderService;

    public async Task<Unit> Handle(CloseOrderCommand request, CancellationToken cancellationToken)
    {
        await _closeOrderService.Close(request.CloseType, request.OrderId, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}