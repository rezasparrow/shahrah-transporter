using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

public interface IVehicleService
{
    Task AssignDriver(long personId, int vehicleId, int driverId, CancellationToken cancellationToken = default);

    Task UnAssignDriver(long personId, int vehicleId, long driverId, CancellationToken cancellationToken = default);

    Task AddVehicle(AddVehicleDto vehicleDto, long personId, CancellationToken cancellationToken = default);

    Task EditVehicle(EditVehicleDto vehicleDto, long personId, CancellationToken cancellationToken = default);

    Task RemoveVehicle(int vehicleId, long personId, CancellationToken cancellationToken = default);
}