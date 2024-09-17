using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicle;

public class GetVehicleQuery : IRequest<VehicleForEditDto>
{
    public GetVehicleQuery(int id, long personId)
    {
        Id = id;
        PersonId = personId;
    }

    public int Id { get; }
    public long PersonId { get; }
}