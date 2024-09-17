using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AssignDriver;

/// <summary>
/// تخصیص راننده به ناوگان
/// </summary>
public class AssignDriverCommand : IRequest<Unit>, ITransactionalCommand
{
    public AssignDriverCommand(long personId, int vehicleId, int driverId)
    {
        VehicleId = vehicleId;
        DriverId = driverId;
        PersonId = personId;
    }

    public long PersonId { get; }

    public int VehicleId { get; }
    public int DriverId { get; }
}