using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AssignDriver;

/// <summary>
/// تخصیص راننده به ناوگان
/// </summary>
public class AssignDriverCommand(long personId, int vehicleId, int driverId) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;

    public int VehicleId { get; } = vehicleId;
    public int DriverId { get; } = driverId;
}