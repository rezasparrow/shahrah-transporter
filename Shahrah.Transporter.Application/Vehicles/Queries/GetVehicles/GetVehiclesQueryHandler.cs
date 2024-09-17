using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Vehicles.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetVehiclesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicles = _dbContext.Vehicles
            .Include(p => p.Transporter)
            .Include(p => p.Truck)
            .Include(p => p.VehicleOptionItems)
            .ThenInclude(p => p.OptionItem)
            .Where(x => x.TransporterId == person.TransporterId)
            .Select(item => new VehicleDto
            {
                Id = item.Id,
                PlateNumber = new PlateNumberDto(item.PlateNumber),
                TruckTypeTitle = item.Truck.Title,
                NetLoadingCapacity = item.NetLoadingCapacity,
                GrossLoadingCapacity = item.GrossLoadingCapacity,
                VehicleOptionItemTitles = item.VehicleOptionItems.Select(q => q.OptionItem.Value).ToList(),
                AssigningStatus = item.DriverId != null ? DriverAssigningStatus.Assinged : DriverAssigningStatus.Unassigned,
                DriverId = item.DriverId,
                DriverFirstName = item.DriverFirstName,
                DriverLastName = item.DriverLastName,
                DriverNationalCode = item.DriverNationalCode
            });

        return await vehicles.ToListAsync(cancellationToken);
    }
}