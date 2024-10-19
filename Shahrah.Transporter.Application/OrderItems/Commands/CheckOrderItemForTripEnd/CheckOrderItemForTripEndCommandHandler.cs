using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForTripEnd;

public class CheckOrderItemForTripEndCommandHandler(IOrderItemService orderItemService) : IRequestHandler<CheckOrderItemForTripEndCommand, Unit>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task<Unit> Handle(CheckOrderItemForTripEndCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.GotoNextStateIfTripEnded(request.OrderItemId, cancellationToken);
        return Unit.Value;
    }
}