using MediatR;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;

internal class AddVehicleCommandHandler : IRequestHandler<AddVehicleCommand, Unit>
{
    private readonly IVehicleService _vehicleService;

    public AddVehicleCommandHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<Unit> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
    {
        await _vehicleService.AddVehicle(request.Vehicle, request.PersonId, cancellationToken);
        return Unit.Value;
    }
}