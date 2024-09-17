using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Commands.AgentEdit;

/// <summary>
/// ویرایش ایجنت
/// </summary>
public class AgentEditCommand : IRequest<Unit>, ITransactionalCommand
{
    public int AgentId { get; set; }
    public long PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
}