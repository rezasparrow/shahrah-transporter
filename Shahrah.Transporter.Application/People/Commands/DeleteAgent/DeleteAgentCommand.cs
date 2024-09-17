using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.DeleteAgent;

public class DeleteAgentCommand : IRequest<Unit>, ITransactionalCommand
{
    public long AgentId { get; set; }
    public long PersonId { get; set; }
}