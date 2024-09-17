using MediatR;
using Shahrah.Transporter.Application.People.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.People.Queries.GetAgents;

public class GetAgentsQuery : IRequest<IEnumerable<PersonDto>>
{
    public long PersonId { get; }

    public GetAgentsQuery(long personId)
    {
        PersonId = personId;
    }
}