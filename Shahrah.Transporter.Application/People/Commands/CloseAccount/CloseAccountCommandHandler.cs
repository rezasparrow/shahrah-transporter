using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.CloseAccount;

internal class CloseAccountCommandHandler(IPersonService personService) : IRequestHandler<CloseAccountCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        await _personService.CloseAccount(request.PersonId, cancellationToken);
        return Unit.Value;
    }
}