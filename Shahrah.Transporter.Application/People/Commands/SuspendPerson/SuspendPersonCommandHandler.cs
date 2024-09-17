using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.SuspendPerson;

public class SuspendPersonCommandHandler : IRequestHandler<SuspendPersonCommand, Unit>
{
    private readonly IPersonService _personService;

    public SuspendPersonCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(SuspendPersonCommand request, CancellationToken cancellationToken)
    {
        await _personService.Suspend(request.PersonId, cancellationToken);
        return Unit.Value;
    }
}