using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentAdd;

/// <summary>
/// ثبت ایجنت
/// </summary>
public class AgentAddCommand : IRequest<Unit>, ITransactionalCommand
{
    public long PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }
    public string NationalCode { get; set; }
}