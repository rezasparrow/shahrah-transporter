using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;

public class RegisterWaybillCodeCommandHandler(IOrderItemService orderItemService) : IRequestHandler<RegisterWaybillCodeCommand>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task Handle(RegisterWaybillCodeCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.RegisterWaybillCode(request.OrderItemId, request.PersonId, request.WaybillCode,cancellationToken);
    }
}