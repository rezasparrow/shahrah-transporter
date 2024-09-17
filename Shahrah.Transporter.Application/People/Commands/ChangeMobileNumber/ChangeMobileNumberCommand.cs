using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.ChangeMobileNumber;

public class ChangeMobileNumberCommand : IRequest<Unit>, ITransactionalCommand
{
    public ChangeMobileNumberCommand(long personId, string mobileNumber, string otp)
    {
        PersonId = personId;
        MobileNumber = mobileNumber;
        Otp = otp;
    }

    public long PersonId { get; }
    public string MobileNumber { get; }
    public string Otp { get; set; }
}