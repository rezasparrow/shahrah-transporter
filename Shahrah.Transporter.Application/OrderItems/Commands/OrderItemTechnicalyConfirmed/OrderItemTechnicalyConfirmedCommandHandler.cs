using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.OrderItemTechnicalyConfirmed;

public class OrderItemTechnicalyConfirmedCommandHandler : IRequestHandler<OrderItemTechnicalyConfirmedCommand>
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemTechnicalyConfirmedCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task Handle(OrderItemTechnicalyConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmTechnicalApprove(request.OrderItemId, request.PersonId, cancellationToken);
    }
}