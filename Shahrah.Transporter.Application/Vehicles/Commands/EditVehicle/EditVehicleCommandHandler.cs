using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;

internal class EditVehicleCommandHandler(IVehicleService vehicleService) : IRequestHandler<EditVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService = vehicleService;

    public async Task<Unit> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.EditVehicle(request.Vehicle, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}