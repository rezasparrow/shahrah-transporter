﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetOptions;

public class GetOptionsQueryHandler : IRequestHandler<GetOptionsQuery, IEnumerable<OptionDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetOptionsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<OptionDto>> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Options.Include(q => q.Items)
            .Select(item =>
                new OptionDto
                {
                    Title = item.Title,
                    Type = (OptionTypeDto)item.Type,
                    Items = item.Items.Select(q => new OptionItemDto
                    {
                        Id = q.Id,
                        Value = q.Value
                    }).ToList()
                })
            .ToListAsync(cancellationToken);
    }
}