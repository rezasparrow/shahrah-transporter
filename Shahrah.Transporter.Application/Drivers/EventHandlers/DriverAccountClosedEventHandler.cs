using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Drivers.EventHandlers;

public class DriverAccountClosedEventHandler : KafkaEventHandler<DriverAccountClosedEvent>
{
    private readonly IVehicleService _vehicleService;
    private readonly IApplicationDbContext _dbContext;

    public DriverAccountClosedEventHandler(IApplicationDbContext dbContext, IVehicleService vehicleService)
    {
        _dbContext = dbContext;
        _vehicleService = vehicleService;
    }

    public override async Task Handle(DriverAccountClosedEvent message)
    {
        var vehicle = await _dbContext.Vehicles.SingleOrDefaultAsync(x => x.DriverId == message.DriverId);
        if (vehicle == null)
            return;

        await _vehicleService.UnAssignDriver(vehicle.TransporterId, vehicle.Id, message.DriverId);
    }
}