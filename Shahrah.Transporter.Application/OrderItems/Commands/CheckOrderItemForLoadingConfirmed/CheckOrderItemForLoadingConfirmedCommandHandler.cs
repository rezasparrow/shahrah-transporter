using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForLoadingConfirmed;

public class CheckOrderItemForLoadingConfirmedCommandHandler(IOrderItemService orderItemService) : IRequestHandler<CheckOrderItemForLoadingConfirmedCommand, Unit>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task<Unit> Handle(CheckOrderItemForLoadingConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.GotoNextStateIfLoadingConfirmed(request.OrderItemId, cancellationToken);
        return Unit.Value;
    }
}