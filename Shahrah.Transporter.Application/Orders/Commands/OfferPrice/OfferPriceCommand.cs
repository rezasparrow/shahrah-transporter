using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.OfferPrice;

/// <summary>
/// ثبت قیمت توسط تی سی برای سفارش
/// </summary>
public class OfferPriceCommand(long personId, int orderId, decimal price) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public int OrderId { get; } = orderId;
    public decimal Price { get; } = price;
}