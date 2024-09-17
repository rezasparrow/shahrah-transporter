using Microsoft.EntityFrameworkCore;
using Parbad;
using Shahrah.Framework.Events;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Payment;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.FinancialTransactions.Services;
using Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;
using Shahrah.Transporter.Application.OrderItems.EventPublishers;
using Shahrah.Transporter.Application.OrderItems.Jobs;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using Shahrah.Transporter.Application.Payments.Models;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Services
{
    public class OrderItemPaymentService : IOrderItemPaymentService
    {
        private readonly IOnlinePayment _onlinePayment;
        private readonly AppSettings _appSettings;
        private readonly IJobScheduler _jobScheduler;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMessageBus _messageBus;
        private readonly INotificationService _notificationService;
        private readonly IFinancialTransactionService _financialTransactionService;
        private readonly IPersonService _personService;
        private readonly OrderItemPaidEventPublisher _orderItemPaidEventPublisher;

        public OrderItemPaymentService(IOnlinePayment onlinePayment, AppSettings appSettings, IJobScheduler jobScheduler, IApplicationDbContext dbContext, IMessageBus messageBus, IFinancialTransactionService financialTransactionService, IPersonService personService, OrderItemPaidEventPublisher orderItemPaidEventPublisher)
        {
            _onlinePayment = onlinePayment;
            _appSettings = appSettings;
            _jobScheduler = jobScheduler;
            _dbContext = dbContext;
            _messageBus = messageBus;
            _financialTransactionService = financialTransactionService;
            _personService = personService;
            _orderItemPaidEventPublisher = orderItemPaidEventPublisher;
        }

        public async Task<IPaymentRequestResult> Pay(IList<int> orderItemsId, GatewayType gateway, CancellationToken cancellationToken = default)
        {
            var waitingForPaymentOrderItems = await _dbContext.OrderItems
                .Include(x => x.Order)
                .Where(q => orderItemsId.Contains(q.Id) && q.Status == OrderItemStatus.PendingPayment || q.Status == OrderItemStatus.AttemptToPay)
                .ToListAsync(cancellationToken);

            if (!waitingForPaymentOrderItems.Any()) throw new Exception("No order item found for pay.");

            var actualAmount = CalculatePayAmount(waitingForPaymentOrderItems);
            var paymentFirstHandShakeResult = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetAmount(actualAmount)
                    .SetCallbackUrl(_appSettings.PaymentUrl)
                    .SetGateway(gateway.ToString())
                    .UseAutoRandomTrackingNumber();
            }, cancellationToken);

            var payment = (await _dbContext.Payments.AddAsync(new Payment
            {
                TrackingNumber = paymentFirstHandShakeResult.TrackingNumber,
                Amount = paymentFirstHandShakeResult.Amount,
                GatewayName = paymentFirstHandShakeResult.GatewayName,
                GatewayAccountName = paymentFirstHandShakeResult.GatewayAccountName
            }, cancellationToken)).Entity;

            foreach (var orderItem in waitingForPaymentOrderItems)
            {
                if (orderItem.Status == OrderItemStatus.PendingPayment)
                {
                    orderItem.Status = OrderItemStatus.AttemptToPay;
                    orderItem.PaymentDeadlineExpiredTime = DateTime.Now.AddSeconds(_appSettings.PaymentInBankGatewayDuration);
                    await _messageBus.Publish(new OrderItemAttemptToPayEvent(orderItem.Order.CorrelationId, orderItem.Id), cancellationToken: cancellationToken);
                    _jobScheduler.Remove<OrderItemPendingPaymentExpiredJob>(orderItem.Id.ToString());
                     _jobScheduler.ScheduleAt<OrderItemAttemptToPayExpiredJob>(builder =>
                        builder.WithIdentifier(orderItem.Id.ToString())
                            .WithSeconds(_appSettings.PaymentInBankGatewayDuration)
                            .WithData(new Dictionary<string, string> { { "OrderItemId", orderItem.Id.ToString() } }));
                }

                orderItem.Payment = payment;
                _dbContext.OrderItems.Update(orderItem);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return paymentFirstHandShakeResult;
        }

        public async Task PayByWallet(long personId, List<int> orderItemIdentities, CancellationToken cancellationToken = default)
        {
            var orderItems = await _dbContext.OrderItems
                .Include(r => r.Order)
                .Where(x => orderItemIdentities.Contains(x.Id)).ToListAsync(cancellationToken);

            var orderId = orderItems.First().OrderId;

            foreach (var orderItem in orderItems)
                 _jobScheduler.Remove<OrderItemPendingPaymentExpiredJob>(orderItem.Id.ToString());

            var payAmount = CalculatePayAmount(orderItems);

            var balance = await _personService.GetCashBalance(personId);

            if (balance - payAmount < 0)
                throw new DomainException(ErrorMessageResource.TransporterBalanceNotEnough);

            var description = $"پرداخت برای آیتمهای {string.Join("-", orderItemIdentities)} از سفارش {orderId} .";

            var referenceId = Guid.NewGuid();

            await _financialTransactionService.CreateTransaction(
                new FinancialTransactionBuilder(personId, payAmount)
                    .WithTransactionType(FinancialTransactionType.Debit)
                    .WithDescription(description)
                    .WithRefrenceId(referenceId));

            foreach (var item in orderItems)
            {
                item.PayTransactionReferenceId = referenceId;
                _dbContext.OrderItems.Update(item);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var item in orderItems)
                await _messageBus.Publish(new OrderItemAttemptToPayEvent(item.Order.CorrelationId, item.Id), cancellationToken: cancellationToken);
        }

        public async Task<VerifyPaymentResultDto> VerifyPayment(IPaymentFetchResult paymentResult, Payment payment,
            CancellationToken cancellationToken = default)
        {
            if (payment.OrderItems.Any(r => r.Status != OrderItemStatus.AttemptToPay))
                return new VerifyPaymentResultDto
                {
                    IsSucceed = false,
                    Message = "به دلیل اتمام مهلت پرداخت حداقل یکی از آیتمهای پرداخت، پرداخت ناموفق بود. لطفا مجدد سعی کنید."
                };

            var verifyResult = await _onlinePayment.VerifyAsync(paymentResult, cancellationToken);
            if (!verifyResult.IsSucceed)
                return await HandlePaymentNotSuccess(verifyResult, payment);

            return await HandlePaymentSuccess(verifyResult, payment);
        }

        private async Task<VerifyPaymentResultDto> HandlePaymentSuccess(IPaymentVerifyResult verifyResult, Payment payment)
        {
            payment.IsPaid = true;
            payment.Message = verifyResult.Message;
            payment.TransactionCode = verifyResult.TransactionCode;
            _dbContext.Payments.Update(payment);

            foreach (var orderItem in payment.OrderItems)
            {
                _jobScheduler.Remove<OrderItemAttemptToPayExpiredJob>(orderItem.Id.ToString());
                orderItem.Status = OrderItemStatus.WaitingForLoading;
                orderItem.PaymentDeadlineExpiredTime = null;
                orderItem.PaidAmount = _appSettings.ShahrahCommissionFromFindingDriver * orderItem.OfferedPriced;
                orderItem.PaymentDate=DateTime.Now;
            }

            await _dbContext.SaveChangesAsync();

            foreach(var orderItem in payment.OrderItems)
            {
                await _orderItemPaidEventPublisher.Publish(orderItem.Id);
            }

            return new VerifyPaymentResultDto
            {
                IsSucceed = true,
                Message = verifyResult.Message,
                TrackingNumber = verifyResult.TrackingNumber
            };
        }

        private async Task<VerifyPaymentResultDto> HandlePaymentNotSuccess(IPaymentVerifyResult verifyResult, Payment payment)
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

        private decimal CalculatePayAmount(IEnumerable<OrderItem> orderItems)
        {
            var amount = orderItems.Sum(x => x.OfferedPriced);
            var totalPayAmount = amount * _appSettings.ShahrahCommissionFromFindingDriver;
            return totalPayAmount;
        }
    }
}