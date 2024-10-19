using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.ReSendOrder;

/// <summary>
/// ارسال مجدد سفارش
/// </summary>
public class ReSendOrderCommand(long personId, int orderId, long minimumOfferPrice, long maximumOfferPrice, int vehicleQuantity) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public int OrderId { get; } = orderId;
    public long MinimumOfferPrice { get; } = minimumOfferPrice;
    public long MaximumOfferPrice { get; } = maximumOfferPrice;
    public int VehicleQuantity { get; } = vehicleQuantity;
}