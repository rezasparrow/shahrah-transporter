using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;

public class AddVehicleCommand : IRequest<Unit>, ITransactionalCommand
{
    public AddVehicleCommand(AddVehicleDto vehicle, long personId)
    {
        Vehicle = vehicle;
        PersonId = personId;
    }

    public AddVehicleDto Vehicle { get; }
    public long PersonId { get; }
}