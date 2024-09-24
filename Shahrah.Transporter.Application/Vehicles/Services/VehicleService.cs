using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Requests;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Responses;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.EventPublishers;
using Shahrah.Transporter.Application.Vehicles.Models;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PersonTypeEnum = Shahrah.Transporter.Domain.Enums.PersonTypeEnum;
using Vehicle = Shahrah.Transporter.Domain.Entities.Vehicle;
using VehicleOptionItem = Shahrah.Transporter.Domain.Entities.VehicleOptionItem;

namespace Shahrah.Transporter.Application.Vehicles.Services;

public class VehicleService : IVehicleService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMessageBus _messageBus;
    private readonly TransportersVehicleUpdatedEventPublisher _transportersVehicleUpdatedEventPublisher;

    public VehicleService(IApplicationDbContext dbContext, IMessageBus messageBus, TransportersVehicleUpdatedEventPublisher transportersVehicleUpdatedEventPublisher)
    {
        _dbContext = dbContext;
        _messageBus = messageBus;
        _transportersVehicleUpdatedEventPublisher = transportersVehicleUpdatedEventPublisher;
    }

    public async Task AssignDriver(long personId, int vehicleId, int driverId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .Include(person => person.Transporter)
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles.Include(t => t.VehicleOptionItems)
            .SingleOrDefaultAsync(q => q.Id == vehicleId
                                       && q.TransporterId == person.TransporterId
                                       && q.DriverId == null, cancellationToken);

        if (vehicle == null)
            throw new DomainException(ErrorMessageResource.VehicleNotFound);

        AssignVehicleToDriverRequest request = new AssignVehicleToDriverRequest
        {
            DriverId = driverId,
            TransporterId = person.TransporterId,
            TransporterName = person.Transporter.Name,
            TransporterNationalId = person.Transporter.NationalId,
            TransporterVehicleId = vehicle.Id,
            IsTransporterVehicleOwner = vehicle.IsTransporterVehicleOwner,
            GrossLoadingCapacity = vehicle.GrossLoadingCapacity,
            NetLoadingCapacity = vehicle.NetLoadingCapacity,
            OwnerFirstName = vehicle.OwnerFirstName,
            OwnerLastName = vehicle.OwnerLastName,
            OwnerNationalCode = vehicle.OwnerNationalCode,
            PlateNumber = vehicle.PlateNumber,
            SmartCardExpirationDate = vehicle.SmartCardExpirationDate,
            SmartCardNumber = vehicle.SmartCardNumber,
            TruckId = vehicle.TruckId,
            VIN = vehicle.Vin,
            VehicleOptionItemsIdentitites = vehicle.VehicleOptionItems?.Select(t => t.OptionItemId) ?? new List<int>()
        };
        var response = await _messageBus.Send<AssignVehicleToDriverResponse, AssignVehicleToDriverRequest>(request, cancellationToken: cancellationToken);

        if (response.Result == AssignVehicleToDriverResultEnum.DriverNotFound)
            throw new DomainException(ErrorMessageResource.DriverNotFound);

        if (response.Result == AssignVehicleToDriverResultEnum.DriverHasVehicle)
            throw new DomainException(ErrorMessageResource.DriverHasVehicleOnAssignVehicleToDriver);

        vehicle.DriverId = response.DriverId;
        vehicle.DriverFirstName = response.DriverFirstName;
        vehicle.DriverLastName = response.DriverLastName;
        vehicle.DriverNationalCode = response.DriverNationalCode;
        vehicle.ModifiedDate = DateTime.Now;
        _dbContext.Vehicles.Update(vehicle);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UnAssignDriver(long personId, int vehicleId, long driverId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles.SingleOrDefaultAsync(
            q => q.Id == vehicleId && q.TransporterId == person.TransporterId && q.DriverId != null && q.DriverId == driverId,
            cancellationToken);

        if (vehicle == null)
            throw new DomainException(ErrorMessageResource.VehicleNotFound);

        var response = await _messageBus.Send<UnassignVehicleFromDriverResponse, UnassignVehicleFromDriverRequest>(new UnassignVehicleFromDriverRequest { DriverId = vehicle.DriverId.Value, TransporterVehicleId = vehicle.Id }, cancellationToken: cancellationToken);

        switch (response.Result)
        {
            case UnassignVehicleFromDriverResultEnum.Success:
                await RemoveVehiclesDriver(vehicle);
                break;

            case UnassignVehicleFromDriverResultEnum.DriverHasActiveAction:
                throw new DomainException(ErrorMessageResource.DriverHasActiveActionOnUnAssignVehicleFromDriver);
        }
    }

    public async Task AddVehicle(AddVehicleDto vehicleDto, long personId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(q => q.Vin == vehicleDto.Vin || q.PlateNumber == vehicleDto.PlateNumber.ToString(), cancellationToken);

        if (vehicle != null)
            throw new DomainException(ErrorMessageResource.VehicleAlreadyExist);

        var vehicleEntity = new Vehicle
        {
            PlateNumber = vehicleDto.PlateNumber.ToString(),
            Vin = vehicleDto.Vin,
            SmartCardNumber = vehicleDto.SmartCardNumber,
            SmartCardExpirationDate = vehicleDto.SmartCardExpirationDate,
            TruckId = vehicleDto.TruckTypeId,
            NetLoadingCapacity = vehicleDto.NetLoadingCapacity,
            GrossLoadingCapacity = vehicleDto.GrossLoadingCapacity,
            VehicleOptionItems =
                vehicleDto.VehicleOptionItems?.ConvertAll(x => new VehicleOptionItem { OptionItemId = x }),
            IsTransporterVehicleOwner = vehicleDto.IsTransporterVehicleOwner,
            OwnerFirstName = vehicleDto.OwnerFirstName,
            OwnerLastName = vehicleDto.OwnerLastName,
            OwnerNationalCode = vehicleDto.OwnerNationalCode,
            TransporterId = person.TransporterId
        };

        await _dbContext.Vehicles.AddAsync(vehicleEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task EditVehicle(EditVehicleDto vehicleDto, long personId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles
            .Include(p => p.VehicleOptionItems)
            .SingleOrDefaultAsync(x => x.Id == vehicleDto.VehicleId && x.TransporterId == person.TransporterId, cancellationToken);

        if (vehicle == null)
            throw new DomainException(ErrorMessageResource.VehicleNotFound);

        //If others vehicle has same platenumber or VIN
        var otherVehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(
            q => q.Id != vehicleDto.VehicleId &&
                 (q.Vin == vehicleDto.Vin || q.PlateNumber == vehicleDto.PlateNumber), cancellationToken);

        if (otherVehicle != null)
            throw new DomainException(ErrorMessageResource.VehicleAlreadyExist);

        vehicle.PlateNumber = vehicleDto.PlateNumber;
        vehicle.Vin = vehicleDto.Vin;
        vehicle.SmartCardNumber = vehicleDto.SmartCardNumber;
        vehicle.SmartCardExpirationDate = vehicleDto.SmartCardExpirationDate;
        vehicle.TruckId = vehicleDto.TruckTypeId;
        vehicle.NetLoadingCapacity = vehicleDto.NetLoadingCapacity;
        vehicle.GrossLoadingCapacity = vehicleDto.GrossLoadingCapacity;
        vehicle.IsTransporterVehicleOwner = vehicleDto.IsTransporterVehicleOwner;
        vehicle.OwnerFirstName = vehicleDto.OwnerFirstName;
        vehicle.OwnerLastName = vehicleDto.OwnerLastName;
        vehicle.OwnerNationalCode = vehicleDto.OwnerNationalCode;
        vehicle.ModifiedDate = DateTime.Now;

        var oldListIds = vehicle.VehicleOptionItems.Select(q => q.OptionItemId).ToList();
        var newListIds = vehicleDto.VehicleOptionItems.ToList();

        var addItemIds = newListIds.Except(oldListIds).ToList();
        var removeItemIds = oldListIds.Except(newListIds).ToList();

        foreach (var id in addItemIds)
        {
            var row = vehicleDto.VehicleOptionItems.First(q => q == id);
            _dbContext.VehicleOptionItems.Add(new VehicleOptionItem { OptionItemId = row, VehicleId = vehicle.Id });
        }

        foreach (var id in removeItemIds)
        {
            var row = vehicle.VehicleOptionItems.First(q => q.OptionItemId == id);
            // TODO: Javad Rasouli >> TASK:4539 حذف باید از vehicle اتفاق بیوفته ولی ما _dbContext رو گذاشتیم
            // وقتی به شکل درست میخواهیم پاک کنیم خطا میده
            // رفع خطا به جایی نرسید و مجبور شدم از _dbContext اقدام کنم.
            // واسه همین تو _transportersVehicleUpdatedEventPublisher.Publish(vehicle.Id) هم مجبور شدم یه TODO بزنم که به همین موضوع ربط داره
            _dbContext.VehicleOptionItems.Remove(row);
        }

        _dbContext.Vehicles.Update(vehicle);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _transportersVehicleUpdatedEventPublisher.Publish(vehicle.Id);
    }

    public async Task RemoveVehicle(int vehicleId, long personId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var vehicle = await _dbContext.Vehicles
            .Include(p => p.VehicleOptionItems)
            .SingleOrDefaultAsync(x => x.Id == vehicleId && x.TransporterId == person.TransporterId, cancellationToken);

        if (vehicle == null)
            throw new DomainException(ErrorMessageResource.VehicleNotFound);

        if (vehicle.DriverId.HasValue)
            throw new DomainException(ErrorMessageResource.VehicleHasDriverOnRemove);

        _dbContext.VehicleOptionItems.RemoveRange(vehicle.VehicleOptionItems);
        _dbContext.Vehicles.Remove(vehicle);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task RemoveVehiclesDriver(Vehicle vehicle)
    {
        vehicle.DriverId = null;
        vehicle.DriverFirstName = null;
        vehicle.DriverLastName = null;
        vehicle.DriverNationalCode = null;
        vehicle.ModifiedDate = DateTime.Now;
        _dbContext.Vehicles.Update(vehicle);
        await _dbContext.SaveChangesAsync();
    }
}