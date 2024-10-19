using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.FindDriver;

/// <summary>
/// جستجوی راننده
/// </summary>
public class FindDriverCommand(long personId, int orderId, decimal minimumPrice, decimal maximumPrice, int vehicleRequestedCount) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; set; } = personId;
    public int OrderId { get; set; } = orderId;
    public decimal MinimumPrice { get; set; } = minimumPrice;
    public decimal MaximumPrice { get; set; } = maximumPrice;
    public int VehicleRequestedCount { get; set; } = vehicleRequestedCount;
}