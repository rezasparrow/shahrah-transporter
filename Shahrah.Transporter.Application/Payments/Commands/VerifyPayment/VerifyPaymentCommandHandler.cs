using MediatR;
using Shahrah.Transporter.Application.Payments.Models;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.VerifyPayment;

public class VerifyPaymentCommandHandler(IPaymentService paymentService) : IRequestHandler<VerifyPaymentCommand, VerifyPaymentResultDto>
{
    private readonly IPaymentService _paymentService = paymentService;

    public async Task<VerifyPaymentResultDto> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.VerifyPayment(request.PaymentResult, cancellationToken);
    }
}