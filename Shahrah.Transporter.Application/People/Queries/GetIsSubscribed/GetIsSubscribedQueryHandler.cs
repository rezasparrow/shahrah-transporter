using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Queries.GetIsSubscribed;

public class GetIsSubscribedQueryHandler(IPersonService subscriptionService) : IRequestHandler<GetIsSubscribedQuery, bool>
{
    private readonly IPersonService _subscriptionService = subscriptionService;

    public async Task<bool> Handle(GetIsSubscribedQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.HasSubscription(request.PersonId);
    }
}