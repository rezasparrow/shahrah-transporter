using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCity;

public class GetCityQuery(int cityId) : IRequest<CityDto>
{
    public int CityId { get; } = cityId;
}