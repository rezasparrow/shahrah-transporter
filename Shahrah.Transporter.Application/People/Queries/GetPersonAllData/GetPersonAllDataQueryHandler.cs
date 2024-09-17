using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Queries.GetPersonAllData;

public class GetPersonAllDataQueryHandler : IRequestHandler<GetPersonAllDataQuery, Person>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPersonAllDataQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Person> Handle(GetPersonAllDataQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.MobileNumber))
            return await Task.FromResult<Person>(null);

        return await _dbContext.People
            .Include(p => p.Transporter)
            .SingleOrDefaultAsync(p => p.MobileNumber == request.MobileNumber, cancellationToken);
    }
}