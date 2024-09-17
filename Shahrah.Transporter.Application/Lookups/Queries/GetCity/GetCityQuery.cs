using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCity;

public class GetCityQuery : IRequest<CityDto>
{
    public GetCityQuery(int cityId)
    {
        CityId = cityId;
    }

    public int CityId { get; }
}