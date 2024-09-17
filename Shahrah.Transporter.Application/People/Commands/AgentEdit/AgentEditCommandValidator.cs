using FluentValidation;

namespace Shahrah.Transporter.Application.People.Commands.AgentEdit;

public class AgentEditCommandValidator : AbstractValidator<AgentEditCommand>
{
    public AgentEditCommandValidator()
    {
        RuleFor(t => t.AgentId).NotEmpty();
        RuleFor(t => t.PersonId).NotEmpty();
        RuleFor(t => t.FirstName).NotEmpty();
        RuleFor(t => t.LastName).NotEmpty();
        RuleFor(t => t.NationalCode).NotEmpty();
    }
}