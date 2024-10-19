using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPlans;

public class GetPlansQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPlansQuery, IEnumerable<PlanDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<PlanDto>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _dbContext.Plans.ToListAsync(cancellationToken);
        return plans.Select(item => new PlanDto
        {
            Id = item.Id,
            Title = item.Title,
            Days = item.Days,
            Price = item.Price,
            Description = item.Description
        });
    }
}