using MediatR;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetAgents;

public class GetAgentsQuery(long personId) : IRequest<List<PersonDto>>
{
    public long PersonId { get; } = personId;
}