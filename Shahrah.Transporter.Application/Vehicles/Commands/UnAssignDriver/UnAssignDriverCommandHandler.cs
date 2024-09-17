using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Vehicles.Commands.UnAssignDriver;

internal class UnAssignDriverCommandHandler : IRequestHandler<UnAssignDriverCommand, Unit>
{
    private readonly IVehicleService _vehicleService;

    public UnAssignDriverCommandHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<Unit> Handle(UnAssignDriverCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.UnAssignDriver(request.PersonId, request.VehicleId, request.DriverId, cancellationToken);
        return Unit.Value;
    }
}