using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Models.DataTransferObjects;
using System;
using System.Linq;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Orders.Models;

public class OrderDto
{
    public OrderDto(Order item)
    {
        if (item == null)
            return;

        Id = item.Id;
        Source = new AddressSimpleDto
        {
            CityId = item.Source.CityId,
            CityName = item.Source.City.Name,
            ProvinceId = item.Source.City.ProvinceId,
            ProvinceName = item.Source.City.Province.Name
        };
        Destination = new AddressSimpleDto
        {
            CityId = item.Destination.CityId,
            CityName = item.Destination.City.Name,
            ProvinceId = item.Destination.City.ProvinceId,
            ProvinceName = item.Destination.City.Province.Name
        };
        Value = item.Value;
        TransporterOfferPrice = item.TransporterOfferPrice;
        SenderOfferPrice = item.SenderOfferPrice;
        MinimumOfferPrice = item.MinimumOfferPrice;
        MaximumOfferPrice = item.MaximumOfferPrice;
        VehicleQuantity = item.VehicleQuantity;
        VehicleFoundedQuantity = item.OrderItems.Count;
        Status = item.Status;
        StatusTitle = item.Status.GetDisplayName();
        IsRegisteredByTc = !item.SenderRequestId.HasValue;
        TruckTitle = item.Truck.Title;
        OrderOptionItems = string.Join(",", item.OrderOptionItems.Select(q => q.OptionItem.Value).ToList());
        LoadingDate = item.LoadingDate;
        SearchOrPendingOrPricingDeadlineExpiredTime = item.SearchOrPendingOrPricingDeadlineExpiredTime;
        // TODO: Javad Rasouli >> این مورد باید یکجا مجتمع باشد و CloseOrderCommand نیز برای چک کردن امکان بستن سفارش از همین متد استفاده کند
        IsCloseable = item.OrderItems.All(q => q.Status is OrderItemStatus.Canceled or OrderItemStatus.DriverNotFound or OrderItemStatus.DriverNotRespond or OrderItemStatus.TripEnded) && Status != OrderStatus.Closed && item.PersonId.HasValue;
        TimeToLive = CalculateTimeToLive();
    }

    public int Id { get; set; }
    public AddressSimpleDto Source { get; set; }
    public AddressSimpleDto Destination { get; set; }
    public decimal Value { get; set; }
    public decimal? TransporterOfferPrice { get; set; }
    public decimal? SenderOfferPrice { get; set; }
    public decimal MinimumOfferPrice { get; set; }
    public decimal MaximumOfferPrice { get; set; }
    public int VehicleQuantity { get; set; }
    public int VehicleFoundedQuantity { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusTitle { get; set; }
    public long TimeToLive { get; set; }
    public bool IsRegisteredByTc { get; set; }
    public bool IsCloseable { get; set; }
    public string TruckTitle { get; set; }
    public string OrderOptionItems { get; set; }
    public DateTime LoadingDate { get; set; }
    public DateTime? SearchOrPendingOrPricingDeadlineExpiredTime { get; set; }

    private long CalculateTimeToLive()
    {
        if (SearchOrPendingOrPricingDeadlineExpiredTime.HasValue)
            return (long)(SearchOrPendingOrPricingDeadlineExpiredTime.Value - DateTime.Now).TotalSeconds;
        return 0;
    }
}