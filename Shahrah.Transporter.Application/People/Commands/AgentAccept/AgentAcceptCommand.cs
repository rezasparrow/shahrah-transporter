using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using System;

namespace Shahrah.Transporter.Application.People.Commands.AgentAccept;

/// <summary>
/// ایجنت بعد از لاگین قبول میکنه که ایجنت یه تی سی باشه
/// </summary>
public class AgentAcceptCommand : IRequest<Unit>, ITransactionalCommand
{
    public long AgentId { get; set; }
    public string FirstName { get; }
    public string LastName { get; }
    public string NationalCode { get; }
    public DateTime BirthDate { get; }

    public AgentAcceptCommand(long agentId, DateTime birthDate, string firstName, string lastName, string nationalCode)
    {
        AgentId = agentId;
        BirthDate = birthDate;
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
    }
}