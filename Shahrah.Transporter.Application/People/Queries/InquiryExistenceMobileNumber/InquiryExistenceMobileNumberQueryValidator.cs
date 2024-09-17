using FluentValidation;
using MediatR;
using Shahrah.Framework.Resources;

namespace Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;

public class InquiryExistenceMobileNumberQueryValidator : AbstractValidator<InquiryExistenceMobileNumberQuery>, IRequest<bool>
{
    public InquiryExistenceMobileNumberQueryValidator()
    {
        RuleFor(x => x.MobileNumber).Matches("^09([0-9]{9})$").WithMessage(ErrorMessageResource.MobileFormatNotCorrect);
    }
}