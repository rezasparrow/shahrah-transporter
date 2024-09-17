using FluentValidation;
using Shahrah.Framework.Resources;

namespace Shahrah.Transporter.Application.Orders.Commands.FindDriver;

public class FindDriverCommandValidator : AbstractValidator<FindDriverCommand>
{
    public FindDriverCommandValidator()
    {
        RuleFor(t => t.MaximumPrice).GreaterThanOrEqualTo(t => t.MinimumPrice).WithMessage(ErrorMessageResource.PriceRangeIsInvalid);
    }
}