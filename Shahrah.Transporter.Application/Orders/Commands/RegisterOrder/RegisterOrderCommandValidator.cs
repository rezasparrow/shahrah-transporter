using FluentValidation;
using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Commands.RegisterOrder;

public class RegisterOrderCommandValidator : AbstractValidator<RegisterOrderCommand>
{
    public RegisterOrderCommandValidator()
    {
        RuleFor(x => x.Order.SourceAddress).NotEmpty().Must(Isvalid);
        RuleFor(x => x.Order.DestinationAddress).NotEmpty().Must(Isvalid);
        RuleFor(x => x.Order.VehicleQuantity).GreaterThan(0).WithMessage(ErrorMessageResource.VehicleQuantityLessThanOne);
        RuleFor(x => x.Order.LoadingDate).GreaterThanOrEqualTo(System.DateTime.Today).WithMessage(ErrorMessageResource.LoadingDateLessThanNow);
        RuleFor(x => x.Order.Weight).NotEmpty();
        RuleFor(x => x.Order.Value).NotEmpty();
        RuleFor(x => x.Order.MaximumOfferPrice).GreaterThan(r => r.Order.MinimumOfferPrice).WithMessage(ErrorMessageResource.PricingIsNotValid);

        RuleFor(x => x.Order.SourceAddress.Latitude).NotEqual(x => x.Order.DestinationAddress.Latitude).WithMessage(ErrorMessageResource.AddressSourceIdEqualToDestinationId);
        RuleFor(x => x.Order.SourceAddress.Longitude).NotEqual(x => x.Order.DestinationAddress.Longitude).WithMessage(ErrorMessageResource.AddressSourceIdEqualToDestinationId);
    }

    private static bool Isvalid(RegisterOrderAddressDto item)
    {
        return item.Latitude > 0 && item.Longitude > 0 && item.CityId > 0;
    }
}