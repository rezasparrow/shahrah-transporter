using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Entities;
using SlimMessageBus;
using System;
using System.Linq;
using System.Threading.Tasks;
using OrderOptionItem = Shahrah.Framework.Models.OrderOptionItem;

namespace Shahrah.Transporter.Application.Orders.EventPublishers;

public class SearchedDriverEventPublisher
{
    private readonly IMessageBus _messageBus;

    public SearchedDriverEventPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Publish(Order order)
    {
        var searchedDriverEvent = new SearchedDriverEvent(order.CorrelationId, (int)(order.SearchOrPendingOrPricingDeadlineExpiredTime.Value - DateTime.Now).TotalSeconds)
        {
            Id = order.Id,
            TransporterPerson = new TransporterPerson
            {
                TransporterId = order.Person.Transporter.Id,
                TransporterName = order.Person.Transporter.Name,
                PersonId = order.Person.Id,
                PersonFirstName = order.Person.FirstName,
                PersonLastName = order.Person.LastName,
                PersonNationalCode = order.Person.NationalCode,
                PersonMobileNumber = order.Person.MobileNumber,
                PersonType = (PersonTypeEnum)order.Person.PersonType
            },
            LoadTitle = order.Load.Title,
            PackagingType = order.Package.Title,
            Source = new Location
            {
                ProvinceId = order.Source.City.ProvinceId,
                ProvincName = order.Source.City.Province.Name,
                CityId = order.Source.CityId,
                CityName = order.Source.City.Name,
                Latitude = order.Source.Latitude,
                Longitude = order.Source.Longitude
            },
            Destination = new Location
            {
                ProvinceId = order.Destination.City.ProvinceId,
                ProvincName = order.Destination.City.Province.Name,
                CityId = order.Destination.CityId,
                CityName = order.Destination.City.Name,
                Latitude = order.Destination.Latitude,
                Longitude = order.Destination.Longitude
            },
            MaximumOfferPrice = order.MaximumOfferPrice,
            MinimumOfferPrice = order.MinimumOfferPrice,
            IsWeighStationRequire = order.IsWeighStationRequire,
            Value = order.Value,
            VehicleRequestedCount = order.VehicleQuantityInSearch,
            TruckId = order.TruckId,

            OrderOptionItems = order.OrderOptionItems.Select(x => new OrderOptionItem
            {
                OptionId = x.OptionItem.OptionId,
                OptionItemId = x.OptionItemId,
                OptionItemValue = x.OptionItem.Value,
                OptionTitle = x.OptionItem.Option.Title,
                Type = (OptionTypeEnum)x.OptionItem.Option.Type
            }).ToList()
        };

        await _messageBus.Publish(searchedDriverEvent);
    }
}