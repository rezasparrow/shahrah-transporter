using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.RegisterPerson;

internal class RegisterPersonCommandHandler : IRequestHandler<RegisterPersonCommand, Unit>
{
    private readonly IPersonService _personService;

    public RegisterPersonCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(RegisterPersonCommand request, CancellationToken cancellationToken)
    {
        await _personService.Register(request.RealPerson, cancellationToken);
        return Unit.Value;
    }
}