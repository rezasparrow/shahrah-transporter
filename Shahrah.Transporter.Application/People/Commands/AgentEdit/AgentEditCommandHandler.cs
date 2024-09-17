using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.AgentEdit;

public class AgentEditCommandHandler : IRequestHandler<AgentEditCommand, Unit>
{
    private readonly IPersonService _personService;

    public AgentEditCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(AgentEditCommand request, CancellationToken cancellationToken)
    {
        await _personService.EditAgent(request, cancellationToken);
        return Unit.Value;
    }
}