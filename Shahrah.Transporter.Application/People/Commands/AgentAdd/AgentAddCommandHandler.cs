using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.AgentAdd;

public class AgentAddCommandHandler : IRequestHandler<AgentAddCommand, Unit>
{
    private readonly IPersonService _personService;

    public AgentAddCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(AgentAddCommand request, CancellationToken cancellationToken)
    {
        await _personService.AddAgent(request, cancellationToken);
        return Unit.Value;
    }
}