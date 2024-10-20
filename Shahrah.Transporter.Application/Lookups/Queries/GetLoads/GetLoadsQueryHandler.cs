﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetLoads;

public class GetLoadsQueryHandler : IRequestHandler<GetLoadsQuery, IEnumerable<LoadDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetLoadsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LoadDto>> Handle(GetLoadsQuery request, CancellationToken cancellationToken)
    {
        var loads = await _dbContext.Loads.OrderBy(o => o.Title).ToListAsync(cancellationToken);
        return loads.Select(item => new LoadDto
        {
            Id = item.Id,
            Title = item.Title
        });
    }
}