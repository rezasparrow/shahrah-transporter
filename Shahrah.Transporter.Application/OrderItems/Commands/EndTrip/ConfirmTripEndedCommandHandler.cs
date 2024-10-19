using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.EndTrip;

public class ConfirmTripEndedCommandHandler(IOrderItemService orderItemService) : IRequestHandler<ConfirmTripEndedCommand>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task Handle(ConfirmTripEndedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmTripEnded(request.OrderItemId, request.PersonId, cancellationToken);
    }
}