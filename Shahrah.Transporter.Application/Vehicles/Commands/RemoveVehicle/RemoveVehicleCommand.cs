using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.RemoveVehicle;

public class RemoveVehicleCommand(long personId, int id) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public int Id { get; } = id;
}