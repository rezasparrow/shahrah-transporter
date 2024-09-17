using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCity;

public class GetCityQueryHandler : IRequestHandler<GetCityQuery, CityDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetCityQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CityDto> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities.SingleAsync(x => x.Id == request.CityId, cancellationToken);
        return new CityDto { Id = city.Id, Name = city.Name, ProvinceId = city.ProvinceId };
    }
}