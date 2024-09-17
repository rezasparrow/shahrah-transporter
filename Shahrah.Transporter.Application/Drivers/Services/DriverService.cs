using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Models;
using Shahrah.Framework.Requests;
using Shahrah.Framework.Responses;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Drivers.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using SlimMessageBus;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Drivers.Services;

public class DriverService : IDriverService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMessageBus _bus;

    public DriverService(IApplicationDbContext dbContext, IMessageBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task<AllocateDriverResponse> SendMessageToDriverForAllocateDriver(int vehicleId, long personId, decimal price, Order order, CancellationToken cancellationToken)
    {
        var vehicle = await _dbContext.Vehicles.SingleAsync(
            x => x.Id == vehicleId && x.Transporter.People.Any<Person>(r => r.Id == personId), cancellationToken);

        return await _bus.Send<AllocateDriverResponse, AllocateDriverRequest>(
        new AllocateDriverRequest(order.CorrelationId)
        {
            DriverId = vehicle.DriverId ?? throw new NullReferenceException("vehicle does not have any driver"),
            OrderId = order.Id,
            Price = price,
            TransporterPersonId = personId,
            LoadTitle = order.Load.Title,
            LoadDescription = order.LoadDescription,
            PackagingType = order.Package.Title,
            PackagingTypeDescription = order.PackingTypeDescription,
            Source = new Location
            {
                Latitude = order.Source.Latitude,
                Longitude = order.Source.Longitude,
                CityId = order.Source.CityId,
                ProvinceId = order.Source.City.ProvinceId
            },
            Destination = new Location
            {
                Latitude = order.Destination.Latitude,
                Longitude = order.Destination.Longitude,
                CityId = order.Destination.CityId,
                ProvinceId = order.Destination.City.ProvinceId
            },
            Value = order.Value,
            IsWeighStationRequire = order.IsWeighStationRequire,
            MinimumOfferPrice = order.MinimumOfferPrice,
            MaximumOfferPrice = order.MaximumOfferPrice,
            VehicleQuantity = order.VehicleQuantity,
            TruckId = order.TruckId,
            OptionItems = order.OrderOptionItems?.Select(x => x.OptionItemId)
        }, cancellationToken: cancellationToken);
    }
}