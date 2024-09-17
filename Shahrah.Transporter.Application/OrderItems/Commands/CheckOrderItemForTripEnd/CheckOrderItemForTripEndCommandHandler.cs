using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForTripEnd;

public class CheckOrderItemForTripEndCommandHandler : IRequestHandler<CheckOrderItemForTripEndCommand, Unit>
{
    private readonly IOrderItemService _orderItemService;

    public CheckOrderItemForTripEndCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task<Unit> Handle(CheckOrderItemForTripEndCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.GotoNextStateIfTripEnded(request.OrderItemId, cancellationToken);
        return Unit.Value;
    }
}