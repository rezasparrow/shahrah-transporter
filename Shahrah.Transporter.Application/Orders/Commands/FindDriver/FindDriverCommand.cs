using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.FindDriver;

/// <summary>
/// جستجوی راننده
/// </summary>
public class FindDriverCommand : IRequest<Unit>, ITransactionalCommand
{
    public FindDriverCommand(long personId, int orderId, decimal minimumPrice, decimal maximumPrice, int vehicleRequestedCount)
    {
        PersonId = personId;
        OrderId = orderId;
        MinimumPrice = minimumPrice;
        MaximumPrice = maximumPrice;
        VehicleRequestedCount = vehicleRequestedCount;
    }

    public long PersonId { get; set; }
    public int OrderId { get; set; }
    public decimal MinimumPrice { get; set; }
    public decimal MaximumPrice { get; set; }
    public int VehicleRequestedCount { get; set; }
}