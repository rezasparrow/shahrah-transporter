using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetTruksByLoadWeight;

public class GetTruksByLoadWeightQuery : IRequest<IEnumerable<TruckDto>>
{
    public GetTruksByLoadWeightQuery(double weight)
    {
        Weight = weight;
    }

    public double Weight { get; }
}