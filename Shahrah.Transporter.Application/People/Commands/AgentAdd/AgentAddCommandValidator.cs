using FluentValidation;

namespace Shahrah.Transporter.Application.People.Commands.AgentAdd;

public class AgentAddCommandValidator : AbstractValidator<AgentAddCommand>
{
    public AgentAddCommandValidator()
    {
        RuleFor(t => t.PersonId).NotEmpty();
        RuleFor(t => t.FirstName).NotEmpty();
        RuleFor(t => t.LastName).NotEmpty();
        RuleFor(t => t.MobileNumber).NotEmpty();
        RuleFor(t => t.NationalCode).NotEmpty();
    }
}