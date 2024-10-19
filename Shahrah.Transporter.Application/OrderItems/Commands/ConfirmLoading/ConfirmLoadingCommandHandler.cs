using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.ConfirmLoading;

public class ConfirmLoadingCommandHandler(IOrderItemService orderItemService) : IRequestHandler<ConfirmLoadingCommand>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public async Task Handle(ConfirmLoadingCommand request, CancellationToken cancellationToken)
    {
        await _orderItemService.ConfirmLoading(request.OrderItemId, request.PersonId, cancellationToken);
    }
}