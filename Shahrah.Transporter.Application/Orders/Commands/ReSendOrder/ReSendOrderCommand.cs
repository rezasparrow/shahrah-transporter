using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.ReSendOrder;

/// <summary>
/// ارسال مجدد سفارش
/// </summary>
public class ReSendOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public ReSendOrderCommand(long personId, int orderId, long minimumOfferPrice, long maximumOfferPrice, int vehicleQuantity)
    {
        PersonId = personId;
        OrderId = orderId;
        MinimumOfferPrice = minimumOfferPrice;
        MaximumOfferPrice = maximumOfferPrice;
        VehicleQuantity = vehicleQuantity;
    }

    public long PersonId { get; }
    public int OrderId { get; }
    public long MinimumOfferPrice { get; }
    public long MaximumOfferPrice { get; }
    public int VehicleQuantity { get; }
}