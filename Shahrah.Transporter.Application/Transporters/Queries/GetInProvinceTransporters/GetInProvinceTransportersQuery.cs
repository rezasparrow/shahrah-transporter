using MediatR;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetInProvinceTransporters;

public class GetInProvinceTransportersQuery(double latitude, double longitude) : IRequest<IEnumerable<Person>>
{
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
}