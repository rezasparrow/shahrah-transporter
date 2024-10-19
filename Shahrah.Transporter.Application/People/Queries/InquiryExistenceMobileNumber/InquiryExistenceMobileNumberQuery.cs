using MediatR;

namespace Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;

public class InquiryExistenceMobileNumberQuery(string mobileNumber) : IRequest<bool>
{
    public string MobileNumber { get; set; } = mobileNumber;
}