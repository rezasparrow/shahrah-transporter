using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.EventPublishers;

public class TransportersVehicleUpdatedEventPublisher
{
    private readonly IMessageBus _messageBus;
    private readonly IApplicationDbContext _dbContext;

    public TransportersVehicleUpdatedEventPublisher(IMessageBus messageBus, IApplicationDbContext dbContext = null)
    {
        _messageBus = messageBus;
        _dbContext = dbContext;
    }

    public async Task Publish(int vehicleId)
    {
        var vehicle = await _dbContext.Vehicles
            .Include(vehicle => vehicle.Transporter)
            .Include(vehicle => vehicle.VehicleOptionItems)
            .FirstOrDefaultAsync(vehicle => vehicle.Id == vehicleId);

        var eventModel = new TransportersVehicleUpdatedEvent()
        {
            DriverId = vehicle.DriverId,
            GrossLoadingCapacity = vehicle.GrossLoadingCapacity,
            NetLoadingCapacity = vehicle.NetLoadingCapacity,
            OwnerFirstName = vehicle.OwnerFirstName,
            OwnerLastName = vehicle.OwnerLastName,
            OwnerNationalCode = vehicle.OwnerNationalCode,
            PlateNumber = vehicle.PlateNumber,
            SmartCardExpirationDate = vehicle.SmartCardExpirationDate,
            SmartCardNumber = vehicle.SmartCardNumber,
            TransporterId = vehicle.TransporterId,
            TransporterName = vehicle.Transporter.Name,
            TransporterNationalId = vehicle.Transporter.NationalId,
            IsTransporterVehicleOwner = vehicle.IsTransporterVehicleOwner,
            TransporterVehicleId = vehicle.Id,
            TruckId = vehicle.TruckId,
            Vin = vehicle.Vin,
            // TODO: Javad Rasouli >> فیلتر IsDeleted اضافه است. نباید تو کوئری میومد
            VehicleOptionItemsIdentitites = vehicle.VehicleOptionItems.Where(r => !r.IsDeleted).Select(voi => voi.OptionItemId)
        };

        await _messageBus.Publish(eventModel);
    }
}