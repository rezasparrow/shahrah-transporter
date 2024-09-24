using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;

public class RegisterWaybillCodeCommandHandler : IRequestHandler<RegisterWaybillCodeCommand>
{
    private readonly IOrderItemService _orderItemService;

    public RegisterWaybillCodeCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task Handle(RegisterWaybillCodeCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.RegisterWaybillCode(request.OrderItemId, request.PersonId, request.WaybillCode,cancellationToken);
    }
}