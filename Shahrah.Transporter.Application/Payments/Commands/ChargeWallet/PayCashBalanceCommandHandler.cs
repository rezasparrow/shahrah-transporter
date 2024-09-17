using MediatR;
using Parbad;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Payments.Commands.ChargeWallet;

public class ChargeWalletCommandHandler : IRequestHandler<ChargeWalletCommand, IPaymentRequestResult>
{
    private readonly IPaymentService _paymentService;

    public ChargeWalletCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<IPaymentRequestResult> Handle(ChargeWalletCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.ChargeWallet(request.PersonId, request.SelectedGateway, request.Amount,
            cancellationToken);
    }
}