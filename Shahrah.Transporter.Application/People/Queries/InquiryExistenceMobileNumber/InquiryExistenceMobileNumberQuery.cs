using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;

public class InquiryExistenceMobileNumberQuery : IRequest<bool>
{
    public InquiryExistenceMobileNumberQuery(string mobileNumber)
    {
        MobileNumber = mobileNumber;
    }

    public string MobileNumber { get; set; }
}