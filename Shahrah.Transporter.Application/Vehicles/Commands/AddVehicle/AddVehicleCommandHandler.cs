using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;

internal class AddVehicleCommandHandler(IVehicleService vehicleService) : IRequestHandler<AddVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService = vehicleService;

    public async Task<Unit> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.AddVehicle(request.Vehicle, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}