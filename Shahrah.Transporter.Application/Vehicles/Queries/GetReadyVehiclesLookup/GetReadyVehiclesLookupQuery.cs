using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetReadyVehiclesLookup;

public class GetReadyVehiclesLookupQuery : IRequest<IEnumerable<ReadyVehiclesLookupDto>>
{
    public GetReadyVehiclesLookupQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}