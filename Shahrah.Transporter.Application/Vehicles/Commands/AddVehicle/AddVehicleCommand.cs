using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;

public class AddVehicleCommand(AddVehicleDto vehicle, long personId) : IRequest<Unit>, ITransactionalCommand
{
    public AddVehicleDto Vehicle { get; } = vehicle;
    public long PersonId { get; } = personId;
}