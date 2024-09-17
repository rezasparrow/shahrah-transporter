using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.UnAssignDriver;

/// <summary>
/// عزل راننده از ناوگان
/// </summary>
public class UnAssignDriverCommand : IRequest<Unit>, ITransactionalCommand
{
    public UnAssignDriverCommand(long personId, int vehicleId, long driverId)
    {
        PersonId = personId;
        VehicleId = vehicleId;
        DriverId = driverId;
    }

    public long PersonId { get; }
    public int VehicleId { get; }
    public long DriverId { get; }
}