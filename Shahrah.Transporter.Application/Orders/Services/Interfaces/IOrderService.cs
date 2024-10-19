using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface IOrderService
{
    Task<int> RegisterOrderAsync(RegisterOrderDto orderDto, long personId);

    Task AssignDriverToOrder(long personId, int orderId, int vehicleId, decimal price,
        CancellationToken cancellationToken = default);

    Task FindDriver(long personId, int orderId, decimal minimumPrice, decimal maximumPrice,
        int vehicleRequestedCount, CancellationToken cancellationToken = default);

    Task PendOrder(int orderId, long personId, CancellationToken cancellationToken = default);

    Task ReSendOrder(long personId, int orderId, long minimumOfferPrice, long maximumOfferPrice, int vehicleQuantity,
        CancellationToken cancellationToken = default);

    Task ConfirmTransporterOfferPriceBySender(ConfirmTransporterOfferPriceEvent message);

    Task OrderClosedBySender(int orderId);
}