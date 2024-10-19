using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentAccept;

/// <summary>
/// ایجنت بعد از لاگین قبول میکنه که ایجنت یه تی سی باشه
/// </summary>
public class AgentAcceptCommand(long agentId, DateTime birthDate, string firstName, string lastName, string nationalCode) : IRequest<Unit>, ITransactionalCommand
{
    public long AgentId { get; set; } = agentId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string NationalCode { get; } = nationalCode;
    public DateTime BirthDate { get; } = birthDate;
}