using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Queries.GetCashBalance;

public class GetCashBalanceQueryHandler(IPersonService personService) : IRequestHandler<GetCashBalanceQuery, decimal>
{
    private readonly IPersonService _personService = personService;

    public async Task<decimal> Handle(GetCashBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _personService.GetCashBalance(request.PersonId);
    }
}