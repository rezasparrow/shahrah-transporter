using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.RemoveVehicle;

internal class RemoveVehicleCommandHandler(IVehicleService vehicleService) : IRequestHandler<RemoveVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService = vehicleService;

    public async Task<Unit> Handle(RemoveVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.RemoveVehicle(request.Id, request.PersonId, cancellationToken);

        return Unit.Value;
    }
}