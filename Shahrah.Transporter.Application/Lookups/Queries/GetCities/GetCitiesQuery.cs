using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

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