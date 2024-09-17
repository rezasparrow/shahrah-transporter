using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Queries.GetCashBalance;

public class GetCashBalanceQueryHandler : IRequestHandler<GetCashBalanceQuery, decimal>
{
    private readonly IPersonService _personService;

    public GetCashBalanceQueryHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<decimal> Handle(GetCashBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _personService.GetCashBalance(request.PersonId);
    }
}