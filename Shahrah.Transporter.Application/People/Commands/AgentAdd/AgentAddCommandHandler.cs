using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentAdd;

public class AgentAddCommandHandler(IPersonService personService) : IRequestHandler<AgentAddCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(AgentAddCommand request, CancellationToken cancellationToken)
    {
        await _personService.AddAgent(request, cancellationToken);
        return Unit.Value;
    }
}