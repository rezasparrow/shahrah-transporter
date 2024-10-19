using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.People.Queries.GetAgent;

public class GetAgentQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAgentQuery, PersonDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<PersonDto> Handle(GetAgentQuery request, CancellationToken cancellationToken)
    {
        var nominatedAsAgent = await _dbContext.People.SingleOrDefaultAsync(
            t => t.Id == request.AgentId && t.AgentRegistrationStatus == AgentRegistrationStatus.Pending,
            cancellationToken);

        if (nominatedAsAgent == null) throw new Exception("Transporter agent not found.");
        return new PersonDto(nominatedAsAgent);
    }
}