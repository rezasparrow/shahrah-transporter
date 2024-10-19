using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentNotAccept;

internal class AgentNotAcceptCommandHandler(IPersonService personService) : IRequestHandler<AgentNotAcceptCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(AgentNotAcceptCommand request, CancellationToken cancellationToken)
    {
        await _personService.AgentNotAccept(request.AgentId, cancellationToken);
        return Unit.Value;
    }
}