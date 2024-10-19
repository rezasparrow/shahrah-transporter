using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;

public class EditVehicleCommand(EditVehicleDto vehicle, long personId) : IRequest<Unit>, ITransactionalCommand
{
    public EditVehicleDto Vehicle { get; } = vehicle;
    public long PersonId { get; } = personId;
}