using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetTruksByLoadWeight;

public class GetTruksByLoadWeightQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTruksByLoadWeightQuery, IEnumerable<TruckDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<TruckDto>> Handle(GetTruksByLoadWeightQuery request, CancellationToken cancellationToken)
    {
        // TODO: Javad Rasouli >> شرط چک شود، یکم عجیبه
        var trucks = await (from truck in _dbContext.Trucks
            where truck.MinLoadWeight <= request.Weight &&
                  truck.MaxLoadWeight.HasValue && truck.MaxLoadWeight.Value >= request.Weight
                  ||
                  truck.MinLoadWeight >= request.Weight
                  || truck.MinLoadWeight <= request.Weight && !truck.MaxLoadWeight.HasValue
            select truck).ToListAsync(cancellationToken);

        return trucks.Select(item => new TruckDto
        {
            Id = item.Id,
            Title = item.Title,
            Height = item.Height,
            Length = item.Length,
            Width = item.Width
        });
    }
}