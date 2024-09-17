using MediatR;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItemsByWallet;

public class PayByWalletCommandHandler : IRequestHandler<PayByWalletCommand>
{
    private readonly IOrderItemPaymentService _orderItemPaymentService;

    public PayByWalletCommandHandler(IOrderItemPaymentService orderItemPaymentService)
    {
        _orderItemPaymentService = orderItemPaymentService;
    }

    public async Task<Unit> Handle(PayByWalletCommand request, CancellationToken cancellationToken)
    {
        await _orderItemPaymentService.PayByWallet(request.PersonId, request.OrderItemIdentities, cancellationToken);
        return Unit.Value;
    }
}