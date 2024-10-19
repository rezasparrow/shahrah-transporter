using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.AssignDriverToOrder;

/// <summary>
/// تخصیص راننده به سفارش
/// </summary>
public class AssignDriverToOrderCommand(long personId, int orderId, int vehicleId, decimal price) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public int OrderId { get; } = orderId;
    public int VehicleId { get; } = vehicleId;
    public decimal Price { get; } = price;
}