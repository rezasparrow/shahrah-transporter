using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.RegisterPerson;

internal class RegisterPersonCommandHandler(IPersonService personService) : IRequestHandler<RegisterPersonCommand, Unit>
{
    private readonly IPersonService _personService = personService;

    public async Task<Unit> Handle(RegisterPersonCommand request, CancellationToken cancellationToken)
    {
        await _personService.Register(request.RealPerson, cancellationToken);
        return Unit.Value;
    }
}