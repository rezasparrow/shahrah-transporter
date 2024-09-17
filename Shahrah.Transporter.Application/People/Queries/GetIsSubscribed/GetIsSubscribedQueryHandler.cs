using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Queries.GetIsSubscribed;

public class GetIsSubscribedQueryHandler : IRequestHandler<GetIsSubscribedQuery, bool>
{
    private readonly IPersonService _subscriptionService;

    public GetIsSubscribedQueryHandler(IPersonService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(GetIsSubscribedQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.HasSubscription(request.PersonId);
    }
}