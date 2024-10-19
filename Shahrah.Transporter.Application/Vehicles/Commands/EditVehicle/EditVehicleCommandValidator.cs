using FluentValidation;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Validators;

namespace Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;

public class EditVehicleCommandValidator : AbstractValidator<EditVehicleCommand>
{
    public EditVehicleCommandValidator()
    {
        RuleFor(x => x.Vehicle.VehicleId).NotEmpty();
        RuleFor(x => x.Vehicle.PlateNumber).NotEmpty();
        RuleFor(x => x.Vehicle.Vin).NotEmpty();
        RuleFor(x => x.Vehicle.SmartCardNumber).NotEmpty();
        RuleFor(x => x.Vehicle.SmartCardExpirationDate.Date).GreaterThan(DateTime.Today).WithMessage(ErrorMessageResource.SmartCardLicenseExpired);
        RuleFor(x => x.Vehicle.TruckTypeId).NotEmpty();
        RuleFor(x => x.Vehicle.OwnerFirstName).NotEmpty().When(r => r.Vehicle.IsTransporterVehicleOwner);
        RuleFor(x => x.Vehicle.OwnerLastName).NotEmpty().When(r => r.Vehicle.IsTransporterVehicleOwner);
        RuleFor(x => x.Vehicle.OwnerNationalCode).Must(NationalCodeValidator.IsValid).WithMessage(ErrorMessageResource.NationalCodeFormatNotCorrect).When(r => r.Vehicle.IsTransporterVehicleOwner);
    }
}