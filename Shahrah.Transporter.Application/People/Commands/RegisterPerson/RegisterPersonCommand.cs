using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Commands.RegisterPerson;

public class RegisterPersonCommand : IRequest<Unit>, ITransactionalCommand
{
    public RegisterPersonCommand(Person realPerson)
    {
        RealPerson = realPerson;
    }

    public Person RealPerson { get; }
}