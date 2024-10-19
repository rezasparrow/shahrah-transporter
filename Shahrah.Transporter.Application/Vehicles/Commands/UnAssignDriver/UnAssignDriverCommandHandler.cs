using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.UnAssignDriver;

internal class UnAssignDriverCommandHandler(IVehicleService vehicleService) : IRequestHandler<UnAssignDriverCommand, Unit>
{
    private readonly IVehicleService _vehicleService = vehicleService;

    public async Task<Unit> Handle(UnAssignDriverCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.UnAssignDriver(request.PersonId, request.VehicleId, request.DriverId, cancellationToken);
        return Unit.Value;
    }
}