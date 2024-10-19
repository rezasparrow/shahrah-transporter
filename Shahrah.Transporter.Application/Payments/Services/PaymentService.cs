using Microsoft.EntityFrameworkCore;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using Shahrah.Transporter.Application.Payments.Models;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Payments.Services;

public class PaymentService(AppSettings appSettings, IOnlinePayment onlinePayment, IApplicationDbContext dbContext, IOrderItemPaymentService orderItemPaymentService) : IPaymentService
{
    private readonly AppSettings _appSettings = appSettings;
    private readonly IOnlinePayment _onlinePayment = onlinePayment;
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IOrderItemPaymentService _orderItemPaymentService = orderItemPaymentService;

    public async Task<IPaymentRequestResult> ChargeWallet(long personId, GatewayType gateway, decimal amount, CancellationToken cancellationToken = default)
    {
        var result = await _onlinePayment.RequestAsync(invoice =>
        {
            invoice
                .SetAmount(amount)
                .SetCallbackUrl(_appSettings.PaymentUrl)
                .SetGateway(gateway.ToString())
                .UseAutoRandomTrackingNumber();
        }, cancellationToken);

        var payment = new Payment
        {
            TrackingNumber = result.TrackingNumber,
            Amount = result.Amount,
            GatewayName = result.GatewayName,
            GatewayAccountName = result.GatewayAccountName,
            PersonId = personId
        };

        await _dbContext.Payments.AddAsync(payment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<IPaymentRequestResult> RegisterPaymentForSubscription(long personId, int planId, GatewayType gateway, CancellationToken cancellationToken = default)
    {
        var plan = await _dbContext.Plans.SingleAsync(x => x.Id == planId,
            cancellationToken);

        var amount = plan.Price;
        var result = await _onlinePayment.RequestAsync(invoice =>
        {
            invoice
                .SetAmount(amount)
                .SetCallbackUrl(_appSettings.PaymentUrl)
                .SetGateway(gateway.ToString())
                .UseAutoRandomTrackingNumber();
        }, cancellationToken);

        var payment = new Payment
        {
            TrackingNumber = result.TrackingNumber,
            Amount = result.Amount,
            GatewayName = result.GatewayName,
            GatewayAccountName = result.GatewayAccountName,
            Subscription = new Subscription
            {
                PlanId = plan.Id,
                PersonId = personId,
                ExpirationDate = DateTime.Today.AddDays(plan.Days),
                Status = SubscriptionStatus.PendingPayment
            }
        };
        await _dbContext.Payments.AddAsync(payment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<VerifyPaymentResultDto> VerifyPayment(IPaymentFetchResult paymentResult, CancellationToken cancellationToken = default)
    {
        if (paymentResult.Status != PaymentFetchResultStatus.ReadyForVerifying)
            return new VerifyPaymentResultDto
            {
                IsSucceed = false,
                Message = "خطایی در فرایند اجرای تراکنش رخ داده است."
            };

        var payment = await _dbContext.Payments
            .Include(t => t.OrderItems).ThenInclude(t => t.Order).ThenInclude(order=> order.Person).ThenInclude(person=> person.Transporter)
            .Include(p => p.Subscription)
            .Include(p => p.Subscription)
            .SingleAsync(p => p.TrackingNumber == paymentResult.TrackingNumber, cancellationToken);

        if (payment.OrderItems.Any())
            return await _orderItemPaymentService.VerifyPayment(paymentResult, payment, cancellationToken);

        if (payment.SubscriptionId.HasValue)
            return await VerifyBuySubscription(paymentResult, payment, cancellationToken);

        return await VerifyChargeCashBalance(paymentResult, payment, cancellationToken);
    }

    private async Task<VerifyPaymentResultDto> VerifyBuySubscription(IPaymentFetchResult paymentResult, Payment payment, CancellationToken cancellationToken = default)
    {
        var verifyResult = await _onlinePayment.VerifyAsync(paymentResult, cancellationToken);
        if (!verifyResult.IsSucceed)
            return await HandleSubscriptionPaymentNotSuccess(verifyResult, payment);

        return await HandleSubscriptionPaymentSuccess(verifyResult, payment);
    }

    private async Task<VerifyPaymentResultDto> VerifyChargeCashBalance(IPaymentFetchResult paymentResult, Payment payment, CancellationToken cancellationToken = default)
    {
        var verifyResult = await _onlinePayment.VerifyAsync(paymentResult, cancellationToken);
        if (!verifyResult.IsSucceed)
            return await HandleCashBalancePaymentNotSuccess(verifyResult, payment);

        return await HandleCashBalancePaymentSuccess(verifyResult, payment);
    }

    private async Task<VerifyPaymentResultDto> HandleSubscriptionPaymentSuccess(IPaymentVerifyResult verifyResult, Payment payment)
    {
        payment.IsPaid = true;
        payment.Message = verifyResult.Message;
        payment.TransactionCode = verifyResult.TransactionCode;
        payment.Subscription.Status = SubscriptionStatus.Paid;

        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();

        return new VerifyPaymentResultDto
        {
            IsSucceed = true,
            Message = verifyResult.Message,
            TrackingNumber = verifyResult.TrackingNumber
        };
    }

    private async Task<VerifyPaymentResultDto> HandleSubscriptionPaymentNotSuccess(IPaymentVerifyResult verifyResult, Payment payment)
    {
        payment.IsPaid = false;
        payment.Message = verifyResult.Message;
        payment.TransactionCode = verifyResult.TransactionCode;
        payment.Subscription.Status = SubscriptionStatus.Canceled;

        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();

        return new VerifyPaymentResultDto
        {
            IsSucceed = false,
            Message = verifyResult.Message,
            TrackingNumber = verifyResult.TrackingNumber
        };
    }

    private async Task<VerifyPaymentResultDto> HandleCashBalancePaymentSuccess(IPaymentVerifyResult verifyResult, Payment payment)
    {
        payment.IsPaid = true;
        payment.Message = verifyResult.Message;
        payment.TransactionCode = verifyResult.TransactionCode;
        _dbContext.Payments.Update(payment);

        var transaction = new FinancialTransaction
        {
            PersonId = payment.PersonId.Value,
            Amount = payment.Amount,
            Description = "واریز به کیف پول",
            TransactionType = FinancialTransactionType.Credit
        };
        await _dbContext.FinancialTransactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();

        return new VerifyPaymentResultDto
        {
            IsSucceed = true,
            Message = verifyResult.Message,
            TrackingNumber = verifyResult.TrackingNumber
        };
    }

    private async Task<VerifyPaymentResultDto> HandleCashBalancePaymentNotSuccess(IPaymentVerifyResult verifyResult, Payment payment)
    {
        payment.IsPaid = false;
        payment.Message = verifyResult.Message;
        payment.TransactionCode = verifyResult.TransactionCode;
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();

        return new VerifyPaymentResultDto
        {
            IsSucceed = false,
            Message = verifyResult.Message,
            TrackingNumber = verifyResult.TrackingNumber
        };
    }
}