using MediatR;
using Parbad;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Payments.Models;

namespace Shahrah.Transporter.Application.Payments.Commands.VerifyPayment;

/// <summary>
/// تائید یا عدم تائید پرداخت
/// </summary>
public class VerifyPaymentCommand(IPaymentFetchResult paymentResult) : IRequest<VerifyPaymentResultDto>, ITransactionalCommand
{
    public IPaymentFetchResult PaymentResult { get; } = paymentResult;
}