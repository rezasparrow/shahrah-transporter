using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.Orders.EventPublishers;

public class OrderRegisteredEventPublisher(IMessageBus messageBus, IOrderQueryService orderQueryService)
{
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IOrderQueryService _orderQueryService = orderQueryService;

    public async Task Publish(int orderId, long? personId)
    {
        var dbOrder = await _orderQueryService.GetOrderWithAllDataAsync(personId, orderId);
        var orderRegistered = new OrderRegisteredEvent(dbOrder.CorrelationId)
        {
            Id = dbOrder.Id,
            CreatedDate = dbOrder.CreatedDate,
            ModifiedDate = dbOrder.ModifiedDate,
            TransporterPerson = dbOrder.Person != null ? new TransporterPerson
            {
                TransporterName = dbOrder.Person.Transporter.Name,
                TransporterId = dbOrder.Person.Transporter.Id,
                PersonId = dbOrder.Person.Id,
                PersonFirstName = dbOrder.Person.FirstName,
                PersonLastName = dbOrder.Person.LastName,
                PersonNationalCode = dbOrder.Person.NationalCode,
                PersonMobileNumber = dbOrder.Person.MobileNumber,
                PersonType = (PersonTypeEnum)dbOrder.Person.PersonType
            } : null,
            LoadTitle = dbOrder.Load.Title,
            LoadDescription = dbOrder.LoadDescription,
            PackagingType = dbOrder.Package.Title,
            PackagingTypeDescription = dbOrder.PackingTypeDescription,
            Weight = dbOrder.Weight,
            Description = dbOrder.Description,
            TransporterOfferPrice = dbOrder.TransporterOfferPrice,
            SenderOfferPrice = dbOrder.SenderOfferPrice,
            IsSpecialOffer = dbOrder.IsSpecialOffer,
            SourceId = dbOrder.SourceId,
            DestinationId = dbOrder.DestinationId,
            Source = new Location
            {
                ProvinceId = dbOrder.Source.City.ProvinceId,
                ProvincName = dbOrder.Source.City.Province.Name,
                CityId = dbOrder.Source.CityId,
                CityName = dbOrder.Source.City.Name,
                Latitude = dbOrder.Source.Latitude,
                Longitude = dbOrder.Source.Longitude
            },
            Destination = new Location
            {
                ProvinceId = dbOrder.Destination.City.ProvinceId,
                ProvincName = dbOrder.Destination.City.Province.Name,
                CityId = dbOrder.Destination.CityId,
                CityName = dbOrder.Destination.City.Name,
                Latitude = dbOrder.Destination.Latitude,
                Longitude = dbOrder.Destination.Longitude
            },
            MaximumOfferPrice = dbOrder.MaximumOfferPrice,
            MinimumOfferPrice = dbOrder.MinimumOfferPrice,
            IsWeighStationRequire = dbOrder.IsWeighStationRequire,
            Value = dbOrder.Value,
            VehicleQuantity = dbOrder.VehicleQuantity,
            SendingDate = dbOrder.SendingDate,
            LoadingDate = dbOrder.LoadingDate,
            Status = (OrderStatusEnum)dbOrder.Status,
            TruckId = dbOrder.TruckId,
            SenderRequestId = dbOrder.SenderRequestId,
            SenderName = dbOrder.SenderName,
            SenderMobileNumber = dbOrder.SenderMobileNumber,
            SenderUserId = dbOrder.SenderUserId,

            OrderOptionItems = dbOrder.OrderOptionItems.Select(x => new OrderOptionItem
            {
                OptionId = x.OptionItem.OptionId,
                OptionItemId = x.OptionItemId,
                OptionItemValue = x.OptionItem.Value,
                OptionTitle = x.OptionItem.Option.Title,
                Type = (OptionTypeEnum)x.OptionItem.Option.Type
            }).ToList(),
            Receivers = dbOrder.Receivers.Select(x => new PersonOrder
            {
                PersonId = x.PersonId,
                PersonName = $"{x.Person.FirstName} {x.Person.LastName}",
                OfferedPrice = x.OfferedPrice
            }).ToList()
        };

        await _messageBus.Publish(orderRegistered);
    }
}