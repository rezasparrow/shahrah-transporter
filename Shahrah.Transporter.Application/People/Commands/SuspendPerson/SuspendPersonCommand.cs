using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.SuspendPerson;

public class SuspendPersonCommand : IRequest<Unit>, ITransactionalCommand
{
    public SuspendPersonCommand(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}