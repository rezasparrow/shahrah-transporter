using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.GetIsSubscribed;

public class GetIsSubscribedQuery : IRequest<bool>
{
    public GetIsSubscribedQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}