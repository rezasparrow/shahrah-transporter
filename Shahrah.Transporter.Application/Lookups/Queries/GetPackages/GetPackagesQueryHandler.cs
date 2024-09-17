using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPackages;

public class GetPackagesQueryHandler : IRequestHandler<GetPackagesQuery, IEnumerable<PackageDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPackagesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<PackageDto>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
    {
        var packages = await _dbContext.Packages.OrderBy(o => o.Title).ToListAsync(cancellationToken);
        return packages.Select(item =>
            new PackageDto
            {
                Id = item.Id,
                Title = item.Title,
            });
    }
}