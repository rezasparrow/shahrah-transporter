using FluentValidation;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Validators;

namespace Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;

public class GetDriversQueryValidator : AbstractValidator<GetDriversQuery>
{
    public GetDriversQueryValidator()
    {
        RuleFor(x => x.NationalCode).Must(NationalCodeValidator.IsValid).WithMessage(ErrorMessageResource.NationalCodeFormatNotCorrect);
    }
}