using MediatR;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Commands.ChangeMobileNumber;

internal class ChangeMobileNumberCommandHandler : IRequestHandler<ChangeMobileNumberCommand, Unit>
{
    private readonly IPersonService _personService;

    public ChangeMobileNumberCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<Unit> Handle(ChangeMobileNumberCommand request, CancellationToken cancellationToken)
    {
        await _personService.ChangeMobileNumber(request.PersonId, request.MobileNumber, request.Otp, cancellationToken);
        return Unit.Value;
    }
}