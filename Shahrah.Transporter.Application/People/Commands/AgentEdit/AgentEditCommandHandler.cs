using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentEdit;

public class AgentEditCommandHandler(IPersonService personService) : IRequestHandler<AgentEditCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(AgentEditCommand request, CancellationToken cancellationToken)
    {
        await _personService.EditAgent(request, cancellationToken);
        return Unit.Value;
    }
}