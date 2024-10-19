using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.GetCashBalance;

public class GetCashBalanceQuery(long personId) : IRequest<decimal>
{
    public long PersonId { get; } = personId;
}