using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Payments.Models;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

public interface IOrderItemPaymentService
{
    Task<IPaymentRequestResult> Pay(IList<int> orderItemsId, GatewayType gateway,
        CancellationToken cancellationToken = default);

    Task PayByWallet(long personId, List<int> orderItemIdentities, CancellationToken cancellationToken = default);

    Task<VerifyPaymentResultDto> VerifyPayment(IPaymentFetchResult paymentResult, Payment payment,
        CancellationToken cancellationToken = default);
}