namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface IOrderPricingService
{
    Task OrderPricingFinished(int orderId, CancellationToken cancellationToken = default);

    Task RegisterOfferPrice(int orderId, long personId, decimal price, CancellationToken cancellationToken = default);
}