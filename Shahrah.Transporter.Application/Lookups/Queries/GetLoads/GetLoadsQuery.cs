using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetLoads;

public class GetLoadsQuery : IRequest<IEnumerable<LoadDto>>
{
}