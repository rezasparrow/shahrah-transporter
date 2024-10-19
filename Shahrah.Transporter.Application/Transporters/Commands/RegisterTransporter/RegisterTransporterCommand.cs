using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Models;
using Shahrah.Transporter.Application.Transporters.Models;

namespace Shahrah.Transporter.Application.Transporters.Commands.RegisterTransporter;

public class RegisterTransporterCommand(RegisterTransporterDto transporterDto, RegisterPersonDto personDto) : IRequest<Unit>, ITransactionalCommand
{
    public RegisterTransporterDto Transporter { get; } = transporterDto;
    public RegisterPersonDto Person { get; } = personDto;
}