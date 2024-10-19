using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Models;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetReadyVehiclesLookup;

public class GetReadyVehiclesLookupQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetReadyVehiclesLookupQuery, IEnumerable<ReadyVehiclesLookupDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<ReadyVehiclesLookupDto>> Handle(GetReadyVehiclesLookupQuery request, CancellationToken cancellationToken)
    {
        // TODO: Javad Rasoili >> این دوتا کوءری قابلیت یکی شدن دارد
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        return await _dbContext.Vehicles
            .Include(p => p.Truck)
            .Include(p => p.VehicleOptionItems)
            .ThenInclude(p => p.OptionItem)
            .Where(x => x.TransporterId == person.TransporterId && x.DriverId.HasValue)
            .Select(item => new ReadyVehiclesLookupDto
            {
                Id = item.Id,
                DriverFirstName = item.DriverFirstName,
                DriverLastName = item.DriverLastName,
                TruckTypeTitle = item.Truck.Title,
                VehicleOptionItemTitles = item.VehicleOptionItems.Select(q => q.OptionItem.Value).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}