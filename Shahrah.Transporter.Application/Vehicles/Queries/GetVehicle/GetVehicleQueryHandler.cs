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

namespace Shahrah.Transporter.Application.Vehicles.Queries.GetVehicle;

public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, VehicleForEditDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetVehicleQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VehicleForEditDto> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles
            .Include(p => p.VehicleOptionItems)
            .SingleOrDefaultAsync(x => x.Id == request.Id && x.TransporterId == person.TransporterId, cancellationToken);

        if (vehicle == null)
            throw new DomainException(ErrorMessageResource.VehicleNotFound);

        return new VehicleForEditDto
        {
            Id = vehicle.Id,
            PlateNumber = new PlateNumberDto(vehicle.PlateNumber),
            Vin = vehicle.Vin,
            SmartCardNumber = vehicle.SmartCardNumber,
            SmartCardExpirationDate = vehicle.SmartCardExpirationDate,
            TruckTypeId = vehicle.TruckId,
            NetLoadingCapacity = vehicle.NetLoadingCapacity,
            GrossLoadingCapacity = vehicle.GrossLoadingCapacity,
            VehicleOptionItems = vehicle.VehicleOptionItems.Select(q => q.OptionItemId).ToList(),
            IsTransporterVehicleOwner = vehicle.IsTransporterVehicleOwner,
            OwnerFirstName = vehicle.OwnerFirstName,
            OwnerLastName = vehicle.OwnerLastName,
            OwnerNationalCode = vehicle.OwnerNationalCode
        };
    }
}