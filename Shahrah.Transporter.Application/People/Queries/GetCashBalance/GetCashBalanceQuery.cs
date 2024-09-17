using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.GetCashBalance;

public class GetCashBalanceQuery : IRequest<decimal>
{
    public GetCashBalanceQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}