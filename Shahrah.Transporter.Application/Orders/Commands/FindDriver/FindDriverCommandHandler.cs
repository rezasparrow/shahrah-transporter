using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.FindDriver;

public class FindDriverCommandHandler(IOrderService orderService) : IRequestHandler<FindDriverCommand, Unit>
{
    private readonly IOrderService _orderService = orderService;

    public async Task<Unit> Handle(FindDriverCommand request, CancellationToken cancellationToken)
    {
        await _orderService.FindDriver(request.PersonId, request.OrderId, request.MinimumPrice, request.MaximumPrice,
            request.VehicleRequestedCount, cancellationToken);

        return Unit.Value;
    }
}