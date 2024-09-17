using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.AssignDriverToOrder;

/// <summary>
/// تخصیص راننده به سفارش
/// </summary>
public class AssignDriverToOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public AssignDriverToOrderCommand(long personId, int orderId, int vehicleId, decimal price)
    {
        PersonId = personId;
        OrderId = orderId;
        VehicleId = vehicleId;
        Price = price;
    }

    public long PersonId { get; }
    public int OrderId { get; }
    public int VehicleId { get; }
    public decimal Price { get; }
}