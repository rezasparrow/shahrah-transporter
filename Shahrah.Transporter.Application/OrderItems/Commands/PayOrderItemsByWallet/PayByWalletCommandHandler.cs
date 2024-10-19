using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItemsByWallet;

public class PayByWalletCommandHandler(IOrderItemPaymentService orderItemPaymentService) : IRequestHandler<PayByWalletCommand>
{
    private readonly IOrderItemPaymentService _orderItemPaymentService = orderItemPaymentService;

    public async Task Handle(PayByWalletCommand request, CancellationToken cancellationToken)
    {
        await _orderItemPaymentService.PayByWallet(request.PersonId, request.OrderItemIdentities, cancellationToken);
    }
}