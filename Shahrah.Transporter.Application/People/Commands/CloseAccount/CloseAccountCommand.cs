using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.CloseAccount;

public class CloseAccountCommand(long personId) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; private set; } = personId;
}