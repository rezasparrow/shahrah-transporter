using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetReadyVehiclesLookup;

public class GetReadyVehiclesLookupQuery(long personId) : IRequest<IEnumerable<ReadyVehiclesLookupDto>>
{
    public long PersonId { get; } = personId;
}