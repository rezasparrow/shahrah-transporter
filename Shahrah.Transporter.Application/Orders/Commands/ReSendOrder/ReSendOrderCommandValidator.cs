using FluentValidation;
using Shahrah.Framework.Resources;

namespace Shahrah.Transporter.Application.Orders.Commands.ReSendOrder;

public class ReSendOrderCommandValidator : AbstractValidator<ReSendOrderCommand>
{
    public ReSendOrderCommandValidator()
    {
        RuleFor(x => x.VehicleQuantity).GreaterThan(0).WithMessage(ErrorMessageResource.VehicleQuantityLessThanOne);
        RuleFor(x => x.MaximumOfferPrice).GreaterThan(r => r.MinimumOfferPrice).WithMessage(ErrorMessageResource.PricingIsNotValid);
    }
}