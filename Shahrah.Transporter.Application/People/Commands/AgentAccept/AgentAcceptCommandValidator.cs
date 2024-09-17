using FluentValidation;

namespace Shahrah.Transporter.Application.People.Commands.AgentAccept;

public class AgentAcceptCommandValidator : AbstractValidator<AgentAcceptCommand>
{
    public AgentAcceptCommandValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.NationalCode).NotEmpty();
    }
}