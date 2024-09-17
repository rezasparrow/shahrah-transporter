using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Vehicles.Commands.RemoveVehicle;

internal class RemoveVehicleCommandHandler : IRequestHandler<RemoveVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService;

    public RemoveVehicleCommandHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<Unit> Handle(RemoveVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.RemoveVehicle(request.Id, request.PersonId, cancellationToken);

        return Unit.Value;
    }
}