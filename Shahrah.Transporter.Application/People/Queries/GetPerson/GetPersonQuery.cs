using MediatR;
using Shahrah.Transporter.Application.People.Models;

namespace Shahrah.Transporter.Application.People.Queries.GetPerson;

public class GetPersonQuery(long personId) : IRequest<PersonDto?>
{
    public long PersonId { get; } = personId;
}