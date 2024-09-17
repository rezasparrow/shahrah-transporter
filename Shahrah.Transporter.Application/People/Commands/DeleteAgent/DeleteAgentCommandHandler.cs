using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.DeleteAgent;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, Unit>
{
    private readonly IPersonService _personService;

    public DeleteAgentCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        await _personService.DeleteAgent(request, cancellationToken);
        return Unit.Value;
    }
}