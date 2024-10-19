using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.OrderItemTechnicalyConfirmed;

public class OrderItemTechnicalyConfirmedCommandHandler(IOrderItemService orderItemService) : IRequestHandler<OrderItemTechnicalyConfirmedCommand>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task Handle(OrderItemTechnicalyConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmTechnicalApprove(request.OrderItemId, request.PersonId, cancellationToken);
    }
}