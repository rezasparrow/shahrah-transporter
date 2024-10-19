using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetPerson;

public class GetPersonQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPersonQuery, PersonDto?>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<PersonDto?> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .Where(x=>x.Id == request.PersonId)
            .SingleOrDefaultAsync(person => person.Id == request.PersonId, cancellationToken);

        return person != null ? new PersonDto(person) : null;
    }
}