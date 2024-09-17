using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.OfferPrice;

/// <summary>
/// ثبت قیمت توسط تی سی برای سفارش
/// </summary>
public class OfferPriceCommand : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; }
    public int OrderId { get; }
    public decimal Price { get; }

    public OfferPriceCommand(long personId, int orderId, decimal price)
    {
        PersonId = personId;
        OrderId = orderId;
        Price = price;
    }
}