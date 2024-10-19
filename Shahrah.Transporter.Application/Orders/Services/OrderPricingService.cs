using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.EventPublishers;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.Orders.Services;

public class OrderPricingService(IApplicationDbContext dbContext, IMessageBus messageBus, IPersonService personService, INotificationService notificationService, ICloseOrderService closeOrderService, WinnerTransporterSpecifiedEventPublisher winnerTransporterSpecifiedEventPublisher) : IOrderPricingService
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IPersonService _personService = personService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ICloseOrderService _closeOrderService = closeOrderService;
    private readonly WinnerTransporterSpecifiedEventPublisher _winnerTransporterSpecifiedEventPublisher = winnerTransporterSpecifiedEventPublisher;

    public async Task OrderPricingFinished(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders
            .Include(x => x.OrderItems)
            .Include(x => x.Receivers)
            .SingleAsync(q => q.Id == orderId, cancellationToken);

        if (order.PersonId.HasValue)
            throw new DomainException(ErrorMessageResource.OrderAssignedToAnotherUser);

        if (order.TransporterOfferPrice.HasValue || order.Status >= OrderStatus.WaitingForConfirmOfferedPrice)
            throw new DomainException(ErrorMessageResource.OrderIsPriced);

        var wonTransporterPerson = await _dbContext.PersonOrders
            .Where(x => x.OrderId == orderId && x.OfferedPrice != null)
            .OrderBy(x => x.OfferedPrice).ThenBy(x => x.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (wonTransporterPerson == null)
            await CloseOrderAndNoticeReciversPricingFinished(order.Id, order.Receivers);
        else
            await HandleAssignOrderToWonTransporterPerson(order, wonTransporterPerson.PersonId, wonTransporterPerson.OfferedPrice.Value);

        await RemoveReceivers(order.Receivers);
    }

    public async Task RegisterOfferPrice(int orderId, long personId, decimal price, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.SingleAsync(order => order.Id == orderId, cancellationToken);
        if (order.Status != OrderStatus.Registered)
            throw new DomainException("order state is not valid...");

        if (order.PersonId.HasValue)
            await ApplyTransporterPersonOfferPrice(orderId, personId, price,
                cancellationToken);
        else
            await CollectPriceForAuction(orderId, personId, price, cancellationToken);
    }

    private async Task ApplyTransporterPersonOfferPriceThrowIfIsNotValid(long personId, decimal price, Order order)
    {
        if (order == null)
            throw new DomainException(string.Format(ErrorMessageResource.OrderWithThisIdNotFound));

        if (!order.SenderRequestId.HasValue) throw new DomainException(ErrorMessageResource.CanNotSpecifyOfferPriceForTransporterOrders);

        if (order.Status >= OrderStatus.WaitingForConfirmOfferedPrice) throw new DomainException(ErrorMessageResource.RequestedOperationIsNotPremitedInCurrentSituation);

        if (!await _personService.HasSubscription(personId)) throw new DomainException(ErrorMessageResource.ActivePlanForTransporterNotFound);

        if (price < order.MinimumOfferPrice)
            throw new DomainException(string.Format(ErrorMessageResource.BidPriceCanNotLowerThanMinPrice, order.MinimumOfferPrice));
    }

    private async Task RemoveReceivers(IEnumerable<PersonOrder> receivers)
    {
        _dbContext.PersonOrders.RemoveRange(receivers);
        await _dbContext.SaveChangesAsync();
    }

    private async Task HandleAssignOrderToWonTransporterPerson(Order order, long wonPersonId, decimal offeredPrice)
    {
        order.PersonId = wonPersonId;
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
        await _winnerTransporterSpecifiedEventPublisher.Publish(order.Id);
        await ApplyTransporterPersonOfferPrice(order.Id, wonPersonId, offeredPrice);
        await _notificationService.SendToPerson(wonPersonId, x => x.OfferPriceWinned(order.Id));

        foreach (var receiver in order.Receivers.Where(r => r.PersonId != wonPersonId))
        {
            if (receiver.OfferedPrice.HasValue)
                await _notificationService.SendToPerson(receiver.PersonId, x => x.OfferPriceNotWinned(order.Id));
            else
                await _notificationService.SendToPerson(receiver.PersonId, x => x.PricingFinished(order.Id));

            await _notificationService.RemoveFromTransporterPersonGroup(order.Id, receiver.PersonId);
        }
    }

    private async Task CloseOrderAndNoticeReciversPricingFinished(int orderId, IEnumerable<PersonOrder> receivers)
    {
        await _closeOrderService.Close(CloseOrderTypeEnum.ForceClose, orderId, null);
        foreach (var receiver in receivers)
        {
            await _notificationService.SendToPerson(receiver.PersonId, x => x.PricingFinished(orderId));
            await _notificationService.RemoveFromTransporterPersonGroup(orderId, receiver.PersonId);
        }
    }

    private async Task CollectPriceForAuction(int orderId, long personId, decimal price, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.SingleOrDefaultAsync(order => order.Id == orderId, cancellationToken);
        if (order == null)
            throw new DomainException(string.Format(ErrorMessageResource.OrderWithThisIdNotFound, orderId));

        if (price < order.MinimumOfferPrice)
            throw new DomainException(string.Format(ErrorMessageResource.BidPriceCanNotLowerThanMinPrice, order.MinimumOfferPrice));

        var item = await _dbContext.PersonOrders.SingleOrDefaultAsync(x => x.OrderId == orderId && x.PersonId == personId, cancellationToken);
        if (item == null)
            throw new DomainException(ErrorMessageResource.NotAllowedToBidThisOrder);

        if (item.OfferedPrice.HasValue)
            throw new DomainException(ErrorMessageResource.AlreadyBidOnThisOrder);

        item.OfferedPrice = price;

        _dbContext.PersonOrders.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ApplyTransporterPersonOfferPrice(int orderId, long personId, decimal price, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.SingleAsync(order => order.Id == orderId && order.PersonId == personId, cancellationToken);

        await ApplyTransporterPersonOfferPriceThrowIfIsNotValid(personId, price, order);

        order.TransporterOfferPrice = price;
        if (price >= order.MinimumOfferPrice && price <= order.SenderOfferPrice)
        {
            order.MaximumOfferPrice = price;
            order.Status = OrderStatus.PriceFinalized;
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _messageBus.Publish(new TransporterAcceptedSenderOfferPriceEvent(order.CorrelationId, order.SenderRequestId.Value, price), cancellationToken: cancellationToken);
        }
        else
        {
            order.Status = OrderStatus.WaitingForConfirmOfferedPrice;
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _messageBus.Publish(new TransporterOfferPriceEvent(order.CorrelationId, order.SenderRequestId.Value, price), cancellationToken: cancellationToken);
        }
    }
}