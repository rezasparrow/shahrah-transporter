using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCities;

public class GetCitiesQuery : IRequest<IEnumerable<CityDto>>
{
    public GetCitiesQuery()
    {
    }

    public GetCitiesQuery(int provinceId)
    {
        ProvinceId = provinceId;
    }

    public int? ProvinceId { get; }
}