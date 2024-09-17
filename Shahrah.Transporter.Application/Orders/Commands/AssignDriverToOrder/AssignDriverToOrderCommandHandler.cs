using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Commands.AssignDriverToOrder;

public class AssignDriverToOrderCommandHandler : IRequestHandler<AssignDriverToOrderCommand, Unit>
{
    private readonly IOrderService _orderService;

    public AssignDriverToOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(AssignDriverToOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderService.AssignDriverToOrder(request.PersonId, request.OrderId, request.VehicleId, request.Price,
            cancellationToken);

        return Unit.Value;
    }
}