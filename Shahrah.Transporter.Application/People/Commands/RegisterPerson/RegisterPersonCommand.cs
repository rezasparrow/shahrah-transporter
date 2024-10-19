using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Commands.RegisterPerson;

public class RegisterPersonCommand(Person realPerson) : IRequest<Unit>, ITransactionalCommand
{
    public Person RealPerson { get; } = realPerson;
}