using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.ConfirmLoading;

public class ConfirmLoadingCommandHandler : IRequestHandler<ConfirmLoadingCommand>
{
    private readonly IOrderItemService _orderItemService;

    public ConfirmLoadingCommandHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task Handle(ConfirmLoadingCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmLoading(request.OrderItemId, request.PersonId, cancellationToken);
    }
}