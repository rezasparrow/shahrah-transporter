using Shahrah.Framework.Events;

namespace Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

public interface IOrderItemService
{
    Task<bool> GotoNextStateIfLoadingConfirmed(int orderItemId, CancellationToken cancellationToken = default);

    Task<bool> GotoNextStateIfTripEnded(int orderItemId, CancellationToken cancellationToken = default);

    Task ConfirmLoading(int orderItemId, long personId, CancellationToken cancellationToken = default);

    Task ConfirmTripEnded(int orderItemId, long personId, CancellationToken cancellationToken = default);

    Task ConfirmTechnicalApprove(int orderItemId, long personId, CancellationToken cancellationToken = default);

    Task PendingPaymentExpired(int orderItemId, CancellationToken cancellationToken = default);

    Task RegisterWaybillCode(int orderItemId, long personId, string waybillCode,
        CancellationToken cancellationToken = default);

    Task BidCanceledByDriver(BidCanceledEvent message);

    Task DriverConfirmTripEnded(DriverConfirmedTripEndedEvent message);

    Task DriverConfirmedLoading(DriverConfirmedLoadingEvent message);

    Task SenderConfirmedTripEnded(SenderConfirmedTripEndedEvent message);

    Task SenderConfirmedLoading(SenderConfirmedLoadingEvent message);
}