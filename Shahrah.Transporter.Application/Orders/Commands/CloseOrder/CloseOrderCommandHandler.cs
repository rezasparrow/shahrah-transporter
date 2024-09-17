using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandler : IRequestHandler<CloseOrderCommand, Unit>
{
    private readonly ICloseOrderService _closeOrderService;

    public CloseOrderCommandHandler(ICloseOrderService closeOrderService)
    {
        _closeOrderService = closeOrderService;
    }

    public async Task<Unit> Handle(CloseOrderCommand request, CancellationToken cancellationToken)
    {
        await _closeOrderService.Close(request.CloseType, request.OrderId, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}