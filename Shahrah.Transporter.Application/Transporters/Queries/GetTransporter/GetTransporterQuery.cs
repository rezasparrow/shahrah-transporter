using MediatR;
using Shahrah.Transporter.Application.Transporters.Models;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;

public class GetTransporterQuery(long personId) : IRequest<TransporterDto>
{
    public long PersonId { get; } = personId;
}