using MediatR;
using Parbad;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.ChargeWallet;

public class ChargeWalletCommandHandler(IPaymentService paymentService) : IRequestHandler<ChargeWalletCommand, IPaymentRequestResult>
{
    private readonly IPaymentService _paymentService = paymentService;

    public async Task<IPaymentRequestResult> Handle(ChargeWalletCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.ChargeWallet(request.PersonId, request.SelectedGateway, request.Amount,
            cancellationToken);
    }
}