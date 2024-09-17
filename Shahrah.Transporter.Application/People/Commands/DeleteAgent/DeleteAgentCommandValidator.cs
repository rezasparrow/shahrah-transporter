using FluentValidation;

namespace Shahrah.Transporter.Application.People.Commands.DeleteAgent;

public class DeleteAgentCommandValidator : AbstractValidator<DeleteAgentCommand>
{
    public DeleteAgentCommandValidator()
    {
        RuleFor(t => t.PersonId).NotEmpty();
        RuleFor(t => t.AgentId).NotEmpty();
    }
}