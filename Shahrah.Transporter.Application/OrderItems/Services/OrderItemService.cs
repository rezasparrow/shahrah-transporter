using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.FinancialTransactions.Services;
using Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;
using Shahrah.Transporter.Application.OrderItems.EventPublishers;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using OrderItemStatus = Shahrah.Transporter.Domain.Enums.OrderItemStatus;

namespace Shahrah.Transporter.Application.OrderItems.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMessageBus _messageBus;
    private readonly INotificationService _notificationService;
    private readonly ICloseOrderService _closeOrderService;
    private readonly IFinancialTransactionService _financialTransactionService;
    private readonly OrderItemChangeStateEventPublisher _orderItemChangeStateEventPublisher;

    public OrderItemService(IApplicationDbContext dbContext, IMessageBus messageBus,
        INotificationService notificationService, ICloseOrderService closeOrderService, IFinancialTransactionService financialTransactionService, OrderItemChangeStateEventPublisher orderItemChangeStateEventPublisher)
    {
        _dbContext = dbContext;
        _messageBus = messageBus;
        _notificationService = notificationService;
        _closeOrderService = closeOrderService;
        _financialTransactionService = financialTransactionService;
        _orderItemChangeStateEventPublisher = orderItemChangeStateEventPublisher;
    }

    public async Task<bool> GotoNextStateIfLoadingConfirmed(int orderItemId,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId, cancellationToken);

        if (orderItem == null)
            return false;

        byte confimedCount = 0;

        if (orderItem.IsLoadingConfirmed)
            confimedCount++;

        if (orderItem.IsLoadingConfirmedByDriver)
            confimedCount++;

        if (orderItem.IsLoadingConfirmedBySender)
            confimedCount++;

        if (confimedCount < 2)
            return false;

        orderItem.Status = OrderItemStatus.WaitingForTechnicalConfirmation;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var orderItemLoadingConfirmedEvent =
            new OrderItemLoadingConfirmedEvent(orderItem.Order.CorrelationId,
            orderItem.Id,
            orderItem.BidId,
            orderItem.IsLoadingConfirmed,
            orderItem.IsLoadingConfirmedBySender,
            orderItem.IsLoadingConfirmedByDriver);

        await _messageBus.Publish(orderItemLoadingConfirmedEvent, cancellationToken: cancellationToken);

        if (orderItem.Order.PersonId.HasValue)
            await _notificationService.SendToPerson(orderItem.Order.PersonId.Value,
                x => x.LoadingConfirmed(orderItem.Order.Id, orderItem.Id));

        return true;
    }

    public async Task<bool> GotoNextStateIfTripEnded(int orderItemId, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId, cancellationToken);

        if (orderItem == null)
            return false;

        byte confimedCount = 0;

        if (orderItem.IsTripEnded)
            confimedCount++;

        if (orderItem.IsTripEndedBySender)
            confimedCount++;

        if (orderItem.IsTripEndedByDriver)
            confimedCount++;

        if (confimedCount < 2)
            return false;

        orderItem.Status = OrderItemStatus.TripEnded;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _closeOrderService.Close(
            CloseOrderTypeEnum.TryToClose, orderItem.OrderId, orderItem.Order.PersonId, cancellationToken);

        var orderItemTripEndedEvent =
            new OrderItemTripEndedEvent(orderItem.Order.CorrelationId,
            orderItem.Id,
            orderItem.BidId,
            orderItem.IsTripEnded,
            orderItem.IsTripEndedByDriver,
            orderItem.IsTripEndedBySender,
            DateTime.Now
            );

        await _messageBus.Publish(orderItemTripEndedEvent, cancellationToken: cancellationToken);

        if (orderItem.Order.PersonId.HasValue)
            await _notificationService.SendToPerson(orderItem.Order.PersonId.Value,
                x => x.TripEnded(orderItem.Order.Id, orderItem.Id));

        return true;
    }

    public async Task ConfirmLoading(int orderItemId, long personId, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId && x.Order.PersonId == personId, cancellationToken);

        if (orderItem == null)
            throw new DomainException(ErrorMessageResource.OrderNotFoundError);

        if (orderItem.Status != OrderItemStatus.WaitingForLoading)
            throw new DomainException(ErrorMessageResource.NotInPendingLoading);

        if (orderItem.IsLoadingConfirmed)
            return;

        orderItem.IsLoadingConfirmed = true;
        orderItem.LoadingConfirmationDateTime = DateTime.Now;
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _messageBus.Publish(new TransporterConfirmedLoadingEvent(orderItem.Order.CorrelationId, orderItem.Id, orderItem.BidId), cancellationToken: cancellationToken);

        await GotoNextStateIfLoadingConfirmed(orderItem.Id, cancellationToken);
    }

    public async Task ConfirmTripEnded(int orderItemId, long personId, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId && x.Order.PersonId == personId, cancellationToken);

        if (orderItem == null)
            throw new DomainException(ErrorMessageResource.OrderNotFoundError);

        if (orderItem.Status != OrderItemStatus.TripStarting)
            throw new DomainException(ErrorMessageResource.WaybillCodeNotRegistered);

        if (orderItem.IsTripEnded)
            return;

        orderItem.IsTripEnded = true;
        orderItem.EndTripDateTime = DateTime.Now;
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _messageBus.Publish(new TransporterConfirmedTripEndedEvent(orderItem.Order.CorrelationId, orderItem.Id, orderItem.BidId), cancellationToken: cancellationToken);
        await GotoNextStateIfTripEnded(orderItem.Id, cancellationToken);
    }

    public async Task ConfirmTechnicalApprove(int orderItemId, long personId,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(r => r.Order)
            .SingleOrDefaultAsync(
            orderItem => orderItem.Id == orderItemId && orderItem.Order.PersonId == personId, cancellationToken);

        if (orderItem == null) throw new Exception($"OrderItem with id {orderItemId} was not found.");

        if (orderItem.Status != OrderItemStatus.WaitingForTechnicalConfirmation)
            throw new DomainException(ErrorMessageResource.OrderItemTechnicalyConfirmedError);

        orderItem.Status = OrderItemStatus.WaitingForWaybillRegisteration;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _messageBus.Publish(new OrderItemTechnicalyConfirmedEvent(orderItem.Order.CorrelationId, orderItem.Id),
            cancellationToken: cancellationToken);
    }

    public async Task PendingPaymentExpired(int orderItemId, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == orderItemId, cancellationToken);

        if (orderItem == null)
            return;

        if (orderItem.Status != OrderItemStatus.PendingPayment && orderItem.Status != OrderItemStatus.AttemptToPay)
            return;

        orderItem.Status = OrderItemStatus.Canceled;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, orderItem.OrderId,
            orderItem.Order.PersonId ?? throw new NullReferenceException("personid can not be null..."),
            cancellationToken);

        await _messageBus.Publish(
            new OrderItemPendingPaymentExpiredEvent(orderItem.Order.CorrelationId, orderItem.Id,
                orderItem.Order.SenderRequestId, orderItem.Id, orderItem.BidId), cancellationToken: cancellationToken);
    }

    public async Task RegisterWaybillCode(int orderItemId, long personId, string waybillCode,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(r => r.Order)
            .SingleOrDefaultAsync(orderItem => orderItem.Id == orderItemId && orderItem.Order.PersonId == personId,
                cancellationToken);

        if (orderItem == null) throw new Exception($"no orderItem found with id {orderItemId}");

        if (orderItem.Status != OrderItemStatus.WaitingForWaybillRegisteration)
            throw new DomainException(ErrorMessageResource.WaybillCodeCanRegisterOnlyAfterTechnicalConfirmation);

        orderItem.WaybillCode = waybillCode;
        orderItem.Status = OrderItemStatus.TripStarting;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _messageBus.Publish(
            new WaybillCodeRegisteredEvent(orderItem.Order.CorrelationId, waybillCode, orderItem.Id, orderItem.BidId),
            cancellationToken: cancellationToken);
    }

    public async Task BidCanceledByDriver(BidCanceledEvent message)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.BidId == message.CanceledBid.Id);

        if (orderItem == null)
            return;

        var transactionBuilder = new FinancialTransactionBuilder(orderItem.Order.PersonId ?? throw new NullReferenceException(), message.CancellationPenaltyAmount)
            .WithTransactionType(FinancialTransactionType.Credit)
            .WithDescription($"بابت جریمه کنسلی سفارش شماره {orderItem.OrderId} آیتم شماره {orderItem.Id} از سمت راننده.");

        await _financialTransactionService.CreateTransaction(transactionBuilder);

        orderItem.Status = OrderItemStatus.Canceled;
        _dbContext.OrderItems.Update(orderItem);
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToTransporterPersonGroup(message.OrderId, x => x.BidCanceled(message.OrderId, orderItem.Id));
        await _orderItemChangeStateEventPublisher.Publish(orderItem.Id, null, OrderItemChangeStateReasonEnum.AuctionCanceled);
        await _closeOrderService.Close(CloseOrderTypeEnum.TryToClose, message.OrderId, orderItem.Order.PersonId.Value);
    }

    public async Task DriverConfirmTripEnded(DriverConfirmedTripEndedEvent message)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.BidId == message.BidId);

        if (orderItem == null)
            return;

        orderItem.IsTripEndedByDriver = true;
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToPerson(orderItem.Order.PersonId ?? throw new NullReferenceException(),
                x => x.DriverConfirmTripEnded(orderItem.Order.Id, orderItem.Id));
        await GotoNextStateIfTripEnded(orderItem.Id);
    }

    public async Task DriverConfirmedLoading(DriverConfirmedLoadingEvent message)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.BidId == message.BidId);

        if (orderItem == null)
            return;

        orderItem.IsLoadingConfirmedByDriver = true;
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToPerson(orderItem.Order.PersonId ?? throw new NullReferenceException(),
                x => x.DriverConfirmedLoading(orderItem.Order.Id, orderItem.Id));
        await GotoNextStateIfLoadingConfirmed(orderItem.Id);
    }

    public async Task SenderConfirmedTripEnded(SenderConfirmedTripEndedEvent message)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == message.TransporterOrderItemId);

        if (orderItem == null)
            return;

        orderItem.IsTripEndedBySender = true;
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToPerson(orderItem.Order.PersonId ?? throw new NullReferenceException(),
                x => x.SenderConfirmTripEnded(orderItem.Order.Id, orderItem.Id));
        await GotoNextStateIfTripEnded(orderItem.Id);
    }

    public async Task SenderConfirmedLoading(SenderConfirmedLoadingEvent message)
    {
        var orderItem = await _dbContext.OrderItems
            .Include(x => x.Order)
            .SingleOrDefaultAsync(x => x.Id == message.TransporterOrderItemId);

        if (orderItem == null)
            return;

        orderItem.IsLoadingConfirmedBySender = true;
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToPerson(orderItem.Order.PersonId ?? throw new NullReferenceException(),
               x => x.SenderConfirmedLoading(orderItem.Order.Id, orderItem.Id));
        await GotoNextStateIfLoadingConfirmed(orderItem.Id);
    }
}