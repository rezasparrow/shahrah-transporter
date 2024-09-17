using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvinces;

public class GetProvincesQueryHandler : IRequestHandler<GetProvincesQuery, IEnumerable<ProvinceDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetProvincesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProvinceDto>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Provinces.OrderBy(o => o.Name)
            .Select(x => new ProvinceDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync(cancellationToken);
    }
}