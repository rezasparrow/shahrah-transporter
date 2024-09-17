using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.EndTrip;

public class ConfirmTripEndedCommandHandler : IRequestHandler<ConfirmTripEndedCommand>
{
    private readonly IOrderItemService _orderItemService;

    public ConfirmTripEndedCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task<Unit> Handle(ConfirmTripEndedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmTripEnded(request.OrderItemId, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}