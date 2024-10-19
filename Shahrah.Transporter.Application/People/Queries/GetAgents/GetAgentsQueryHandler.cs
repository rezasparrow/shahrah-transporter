using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.People.Queries.GetAgents;

public class GetAgentsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAgentsQuery, List<PersonDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<List<PersonDto>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var agents = await _dbContext.People.Where(t => t.TransporterId == person.TransporterId)
            
            .ToListAsync(cancellationToken);

        return agents.Select(x => new PersonDto(x)).ToList();
    }
}