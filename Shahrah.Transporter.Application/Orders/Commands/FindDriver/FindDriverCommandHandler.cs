using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Commands.FindDriver;

public class FindDriverCommandHandler : IRequestHandler<FindDriverCommand, Unit>
{
    private readonly IOrderService _orderService;

    public FindDriverCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(FindDriverCommand request, CancellationToken cancellationToken)
    {
        await _orderService.FindDriver(request.PersonId, request.OrderId, request.MinimumPrice, request.MaximumPrice,
            request.VehicleRequestedCount, cancellationToken);

        return Unit.Value;
    }
}