using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Payments.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Payments.Services.Interfaces;

public interface IPaymentService
{
    Task<IPaymentRequestResult> ChargeWallet(long personId, GatewayType gateway, decimal amount,
        CancellationToken cancellationToken = default);

    Task<IPaymentRequestResult> RegisterPaymentForSubscription(long personId, int planId, GatewayType gateway,
        CancellationToken cancellationToken = default);

    Task<VerifyPaymentResultDto> VerifyPayment(IPaymentFetchResult paymentResult,
        CancellationToken cancellationToken = default);
}