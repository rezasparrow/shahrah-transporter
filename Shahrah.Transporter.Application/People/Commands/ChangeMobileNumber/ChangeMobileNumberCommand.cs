using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.ChangeMobileNumber;

public class ChangeMobileNumberCommand(long personId, string mobileNumber, string otp) : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; } = personId;
    public string MobileNumber { get; } = mobileNumber;
    public string Otp { get; set; } = otp;
}