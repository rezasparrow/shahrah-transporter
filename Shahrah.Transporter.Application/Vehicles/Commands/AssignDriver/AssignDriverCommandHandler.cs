using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AssignDriver;

internal class AssignDriverCommandHandler : IRequestHandler<AssignDriverCommand, Unit>
{
    private readonly IVehicleService _vehicleService;

    public AssignDriverCommandHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<Unit> Handle(AssignDriverCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.AssignDriver(request.PersonId, request.VehicleId, request.DriverId, cancellationToken);
        return Unit.Value;
    }
}