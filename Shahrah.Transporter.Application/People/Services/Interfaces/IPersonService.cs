using Shahrah.Transporter.Application.People.Commands.AgentAdd;
using Shahrah.Transporter.Application.People.Commands.AgentEdit;
using Shahrah.Transporter.Application.People.Commands.DeleteAgent;
using Shahrah.Transporter.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Services.Interfaces;

public interface IPersonService
{
    Task<bool> HasSubscription(long personId);

    Task<decimal> GetCashBalance(long personId);

    Task Activate(long personId);

    Task AgentAccept(long agentId, DateTime birthDate, string firstName, string lastName,
        string nationalCode, CancellationToken cancellationToken = default);

    Task AddAgent(AgentAddCommand request, CancellationToken cancellationToken = default);

    Task EditAgent(AgentEditCommand request, CancellationToken cancellationToken = default);

    Task AgentNotAccept(long agentId, CancellationToken cancellationToken = default);

    Task ChangeMobileNumber(long personId, string mobileNumber, string otp,
        CancellationToken cancellationToken = default);

    Task CloseAccount(long personId, CancellationToken cancellationToken = default);

    Task DeleteAgent(DeleteAgentCommand request, CancellationToken cancellationToken = default);

    Task Register(Person realPerson, CancellationToken cancellationToken = default);

    Task Suspend(long personId, CancellationToken cancellationToken = default);
}