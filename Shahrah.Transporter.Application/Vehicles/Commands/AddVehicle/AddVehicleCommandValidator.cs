using FluentValidation;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Validators;
using System;

namespace Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;

public class AddVehicleCommandValidator : AbstractValidator<AddVehicleCommand>
{
    public AddVehicleCommandValidator()
    {
        RuleFor(x => x.Vehicle.PlateNumber).NotEmpty();

        RuleFor(x => x.Vehicle.Vin).NotEmpty();

        RuleFor(x => x.Vehicle.SmartCardNumber).NotEmpty();

        RuleFor(x => x.Vehicle.SmartCardExpirationDate.Date)
            .GreaterThan(DateTime.Today)
            .WithMessage(ErrorMessageResource.SmartCardLicenseExpired);

        RuleFor(x => x.Vehicle.TruckTypeId)
            .NotEmpty();

        RuleFor(x => x.Vehicle.OwnerFirstName)
            .NotEmpty()
            .When(x => !x.Vehicle.IsTransporterVehicleOwner);

        RuleFor(x => x.Vehicle.OwnerLastName)
            .NotEmpty()
            .When(x => !x.Vehicle.IsTransporterVehicleOwner);

        RuleFor(x => x.Vehicle.OwnerNationalCode)
            .Must(NationalCodeValidator.IsValid)
            .WithMessage(ErrorMessageResource.NationalCodeFormatNotCorrect)
            .When(x => !x.Vehicle.IsTransporterVehicleOwner);
    }
}