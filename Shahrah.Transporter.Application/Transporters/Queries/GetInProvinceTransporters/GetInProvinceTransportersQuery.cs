using MediatR;
using Shahrah.Transporter.Domain.Entities;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetInProvinceTransporters;

public class GetInProvinceTransportersQuery : IRequest<IEnumerable<Person>>
{
    public GetInProvinceTransportersQuery(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }
}