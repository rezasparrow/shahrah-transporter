using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCity;

public class GetCityQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCityQuery, CityDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<CityDto> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities.SingleAsync(x => x.Id == request.CityId, cancellationToken);
        return new CityDto { Id = city.Id, Name = city.Name, ProvinceId = city.ProvinceId };
    }
}