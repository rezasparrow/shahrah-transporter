using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;

public class EditVehicleCommand : IRequest<Unit>, ITransactionalCommand
{
    public EditVehicleCommand(EditVehicleDto vehicle, long personId)
    {
        Vehicle = vehicle;
        PersonId = personId;
    }

    public EditVehicleDto Vehicle { get; }
    public long PersonId { get; }
}