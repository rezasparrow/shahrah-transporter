using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.AgentAccept;

public class AgentAcceptCommandHandler : IRequestHandler<AgentAcceptCommand, Unit>
{
    private readonly IPersonService _personService;

    public AgentAcceptCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(AgentAcceptCommand request, CancellationToken cancellationToken)
    {
        await _personService.AgentAccept(request.AgentId, request.BirthDate, request.FirstName, request.LastName,
            request.NationalCode, cancellationToken);

        return Unit.Value;
    }
}