using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<CityDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetCitiesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<City> data;

        if (request.ProvinceId.HasValue)
            data = await _dbContext.Cities.Where(x => x.ProvinceId == request.ProvinceId).OrderBy(o => o.Name).ToListAsync(cancellationToken);
        else
            data = await _dbContext.Cities.OrderBy(o => o.Name).ToListAsync(cancellationToken);

        return data.Select(x => new CityDto
        {
            Id = x.Id,
            Name = x.Name,
            ProvinceId = x.ProvinceId,
        });
    }
}