using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetTrucks;

public class GetTrucksQueryHandler : IRequestHandler<GetTrucksQuery, IEnumerable<TruckDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTrucksQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TruckDto>> Handle(GetTrucksQuery request, CancellationToken cancellationToken)
    {
        var trucks = await _dbContext.Trucks.OrderBy(o => o.Title).ToListAsync(cancellationToken);
        return trucks.Select(item => new TruckDto
        {
            Id = item.Id,
            Title = item.Title,
            Height = item.Height,
            Length = item.Length,
            Width = item.Width
        });
    }
}