using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Models;
using Shahrah.Transporter.Application.Transporters.Models;

namespace Shahrah.Transporter.Application.Transporters.Commands.RegisterTransporter;

public class RegisterTransporterCommand : IRequest<Unit>, ITransactionalCommand
{
    public RegisterTransporterCommand(RegisterTransporterDto transporterDto, RegisterPersonDto personDto)
    {
        Transporter = transporterDto;
        Person = personDto;
    }

    public RegisterTransporterDto Transporter { get; }
    public RegisterPersonDto Person { get; }
}