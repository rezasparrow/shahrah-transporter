using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Queries.GetPersonAllData;

public class GetPersonAllDataQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPersonAllDataQuery, Person>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Person> Handle(GetPersonAllDataQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.MobileNumber))
            return await Task.FromResult<Person>(null);

        return await _dbContext.People
            .Include(p => p.Transporter)
            .SingleOrDefaultAsync(p => p.MobileNumber == request.MobileNumber, cancellationToken);
    }
}