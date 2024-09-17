using MediatR;
using Shahrah.Transporter.Application.Vehicles.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesQuery : IRequest<IEnumerable<VehicleDto>>
{
    public GetVehiclesQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}