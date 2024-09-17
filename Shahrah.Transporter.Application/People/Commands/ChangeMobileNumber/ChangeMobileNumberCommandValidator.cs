using FluentValidation;
using Shahrah.Framework.Resources;

namespace Shahrah.Transporter.Application.People.Commands.ChangeMobileNumber;

public class ChangeMobileNumberCommandValidator : AbstractValidator<ChangeMobileNumberCommand>
{
    public ChangeMobileNumberCommandValidator()
    {
        RuleFor(x => x.MobileNumber).Matches("^09([0-9]{9})$").WithMessage(ErrorMessageResource.MobileFormatNotCorrect);
    }
}