using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Vehicles.Commands.RemoveVehicle;

public class RemoveVehicleCommand : IRequest<Unit>, ITransactionalCommand
{
    public RemoveVehicleCommand(long personId, int id)
    {
        Id = id;
        PersonId = personId;
    }

    public long PersonId { get; }
    public int Id { get; }
}