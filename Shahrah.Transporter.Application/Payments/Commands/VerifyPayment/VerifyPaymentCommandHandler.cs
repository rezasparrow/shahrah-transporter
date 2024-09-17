using MediatR;
using Shahrah.Transporter.Application.Payments.Models;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Payments.Commands.VerifyPayment;

public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, VerifyPaymentResultDto>
{
    private readonly IPaymentService _paymentService;

    public VerifyPaymentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<VerifyPaymentResultDto> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.VerifyPayment(request.PaymentResult, cancellationToken);
    }
}