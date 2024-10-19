using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesQuery(long personId) : IRequest<IEnumerable<VehicleDto>>
{
    public long PersonId { get; } = personId;
}