using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetTruksByLoadWeight;

public class GetTruksByLoadWeightQuery(double weight) : IRequest<IEnumerable<TruckDto>>
{
    public double Weight { get; } = weight;
}