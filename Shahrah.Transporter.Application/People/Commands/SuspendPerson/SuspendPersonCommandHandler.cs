using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.SuspendPerson;

public class SuspendPersonCommandHandler(IPersonService personService) : IRequestHandler<SuspendPersonCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(SuspendPersonCommand request, CancellationToken cancellationToken)
    {
        await _personService.Suspend(request.PersonId, cancellationToken);
        return Unit.Value;
    }
}