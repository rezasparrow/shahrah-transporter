using MediatR;
using Shahrah.Transporter.Application.Transporters.Models;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;

public class GetTransporterQuery : IRequest<TransporterDto>
{
    public GetTransporterQuery(long personId)
    {
        PersonId = personId;
    }

    public long PersonId { get; }
}