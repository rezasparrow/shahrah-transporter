using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.UnAssignDriver;

/// <summary>
/// عزل راننده از ناوگان
/// </summary>
public class UnAssignDriverCommand(long personId, int vehicleId, long driverId) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public int VehicleId { get; } = vehicleId;
    public long DriverId { get; } = driverId;
}