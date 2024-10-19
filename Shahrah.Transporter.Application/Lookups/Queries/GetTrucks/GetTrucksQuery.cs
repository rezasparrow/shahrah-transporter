using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetTrucks;

public class GetTrucksQuery : IRequest<IEnumerable<TruckDto>>
{
}