using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentNotAccept;

/// <summary>
/// ایجنت هنگام اولین لاگین قبول نمیکنه که ایجنت برای تی سی باشه
/// </summary>
public class AgentNotAcceptCommand(long agentId) : IRequest<Unit>, ITransactionalCommand
{
    public long AgentId { get; } = agentId;
}