using System;
using FluentValidation;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Validators;

namespace Shahrah.Transporter.Application.Transporters.Commands.RegisterTransporter;

public class RegisterTransporterCommandValidator : AbstractValidator<RegisterTransporterCommand>
{
    public RegisterTransporterCommandValidator()
    {
        RuleFor(x => x.Transporter.Name).NotEmpty();
        RuleFor(x => x.Transporter.NationalId).Must(NationalIdValidator.IsValid).WithMessage(ErrorMessageResource.NationalIdFormatNotCorrect);
        RuleFor(x => x.Transporter.LicenseSerialNumber).NotEmpty();
        RuleFor(x => x.Transporter.LicenseExpirationDate.Date).GreaterThan(DateTime.Today).WithMessage(ErrorMessageResource.LicenseExpired);
        RuleFor(x => x.Transporter.Address).NotEmpty();
        RuleFor(x => x.Transporter.PostalCode).NotEmpty();

        RuleFor(x => x.Person != null);

        RuleFor(x => x.Person.NationalCode)
            .Must(NationalCodeValidator.IsValid)
            .WithMessage(ErrorMessageResource.NationalCodeFormatNotCorrect);

        RuleFor(x => x.Person.BirthDate.Date)
            .LessThan(DateTime.Today)
            .WithMessage(ErrorMessageResource.BirthDateNotValid);
    }
}