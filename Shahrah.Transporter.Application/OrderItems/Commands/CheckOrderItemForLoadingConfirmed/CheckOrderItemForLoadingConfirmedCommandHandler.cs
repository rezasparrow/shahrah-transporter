using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.CheckOrderItemForLoadingConfirmed;

public class CheckOrderItemForLoadingConfirmedCommandHandler : IRequestHandler<CheckOrderItemForLoadingConfirmedCommand, Unit>
{
    private readonly IOrderItemService _orderItemService;

    public CheckOrderItemForLoadingConfirmedCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task<Unit> Handle(CheckOrderItemForLoadingConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.GotoNextStateIfLoadingConfirmed(request.OrderItemId, cancellationToken);
        return Unit.Value;
    }
}