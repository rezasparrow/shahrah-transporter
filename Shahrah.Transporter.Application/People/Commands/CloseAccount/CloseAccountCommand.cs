using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.CloseAccount;

public class CloseAccountCommand : IRequest<Unit>, ITransactionalCommand
{
    public CloseAccountCommand(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; private set; }
}