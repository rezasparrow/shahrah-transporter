using FluentValidation;
using Shahrah.Framework.Resources;

namespace Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;

public class RegisterWaybillCodeCommandValidator : AbstractValidator<RegisterWaybillCodeCommand>
{
    public RegisterWaybillCodeCommandValidator()
    {
        RuleFor(t => t.WaybillCode)
            .NotNull().WithMessage(ErrorMessageResource.WaybillCodeMustBe17Digit)
            .Matches(@"^\d{17}$").WithMessage(ErrorMessageResource.WaybillCodeMustBe17Digit);
    }
}