using MediatR;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetAgent;

public class GetAgentQuery : IRequest<PersonDto>
{
    public GetAgentQuery(long agentId)
    {
        AgentId = agentId;
    }

    public long AgentId { get; }
}