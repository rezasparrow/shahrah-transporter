using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.DeleteAgent;

public class DeleteAgentCommandHandler(IPersonService personService) : IRequestHandler<DeleteAgentCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        await _personService.DeleteAgent(request, cancellationToken);
        return Unit.Value;
    }
}