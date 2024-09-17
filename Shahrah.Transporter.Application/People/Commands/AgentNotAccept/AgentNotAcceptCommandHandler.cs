using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.AgentNotAccept;

internal class AgentNotAcceptCommandHandler : IRequestHandler<AgentNotAcceptCommand, Unit>
{
    private readonly IPersonService _personService;

    public AgentNotAcceptCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(AgentNotAcceptCommand request, CancellationToken cancellationToken)
    {
        await _personService.AgentNotAccept(request.AgentId, cancellationToken);
        return Unit.Value;
    }
}