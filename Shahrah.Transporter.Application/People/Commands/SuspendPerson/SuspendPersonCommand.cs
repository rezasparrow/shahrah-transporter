using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.SuspendPerson;

public class SuspendPersonCommand(long personId) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
}