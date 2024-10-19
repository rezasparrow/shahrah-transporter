using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application.Drivers.EventHandlers;

public class DriverAccountClosedEventHandler(IApplicationDbContext dbContext, IVehicleService vehicleService) : KafkaEventHandler<DriverAccountClosedEvent>
{
    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public override async Task Handle(DriverAccountClosedEvent message)
    {
        var vehicle = await _dbContext.Vehicles.SingleOrDefaultAsync(x => x.DriverId == message.DriverId);
        if (vehicle == null)
            return;

        await _vehicleService.UnAssignDriver(vehicle.TransporterId, vehicle.Id, message.DriverId);
    }
}