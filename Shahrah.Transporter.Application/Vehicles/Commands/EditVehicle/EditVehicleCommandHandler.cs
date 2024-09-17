using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;

internal class EditVehicleCommandHandler : IRequestHandler<EditVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService;

    public EditVehicleCommandHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<Unit> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.EditVehicle(request.Vehicle, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}