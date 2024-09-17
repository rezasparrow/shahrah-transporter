using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.CloseAccount;

internal class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, Unit>
{
    private readonly IPersonService _personService;

    public CloseAccountCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        await _personService.CloseAccount(request.PersonId, cancellationToken);
        return Unit.Value;
    }
}