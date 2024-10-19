using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicle;

public class GetVehicleQuery(int id, long personId) : IRequest<VehicleForEditDto>
{
    public int Id { get; } = id;
    public long PersonId { get; } = personId;
}