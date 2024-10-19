using MediatR;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetAgent;

public class GetAgentQuery(long agentId) : IRequest<PersonDto>
{
    public long AgentId { get; } = agentId;
}