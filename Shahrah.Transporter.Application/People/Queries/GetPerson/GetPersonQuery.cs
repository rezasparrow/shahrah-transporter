using MediatR;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetPerson;

public class GetPersonQuery : IRequest<PersonDto?>
{
    public GetPersonQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}