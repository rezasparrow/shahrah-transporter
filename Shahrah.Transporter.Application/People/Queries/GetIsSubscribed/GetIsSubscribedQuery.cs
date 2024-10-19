using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.GetIsSubscribed;

public class GetIsSubscribedQuery(long personId) : IRequest<bool>
{
    public long PersonId { get; } = personId;
}