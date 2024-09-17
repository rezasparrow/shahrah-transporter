using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Orders.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class OrderModel
{
    [Display(Name = "SourceAddress", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public AddressLightModel SourceAddress { get; set; }

    [Display(Name = "DestinationAddress", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public AddressLightModel DestinationAddress { get; set; }

    public int LoadId { get; set; }

    public string LoadDescription { get; set; }

    [Display(Name = "LoadingDate", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public DateTime LoadingDate { get; set; }

    public int PackageId { get; set; }

    public string packingTypeDescription { get; set; }

    [Display(Name = "Weight", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public float Weight { get; set; }

    [Display(Name = "Value", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public long Value { get; set; }

    public string Description { get; set; }

    [Display(Name = "TruckType", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int TruckId { get; set; }

    public List<int> VehicleOptionItems { get; set; }

    public bool IsWeighStationrequire { get; set; }

    [Display(Name = "MinimumOfferPrice", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public long MinimumOfferPrice { get; set; }

    [Display(Name = "MaximumOfferPrice", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public long MaximumOfferPrice { get; set; }

    [Display(Name = "VehicleQuantity", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int VehicleQuantity { get; set; }

    public bool IsSpecialOffer { get; set; }

    public RegisterOrderDto ToRegisterOrderDto()
    {
        return new RegisterOrderDto
        {
            SourceAddress = new RegisterOrderAddressDto
            {
                CityId = SourceAddress.CityId,
                Latitude = SourceAddress.Latitude,
                Longitude = SourceAddress.Longitude,
            },
            DestinationAddress = new RegisterOrderAddressDto
            {
                CityId = DestinationAddress.CityId,
                Latitude = DestinationAddress.Latitude,
                Longitude = DestinationAddress.Longitude,
            },
            LoadId = LoadId,
            LoadDescription = LoadDescription,
            LoadingDate = LoadingDate,
            PackageId = PackageId,
            PackingTypeDescription = packingTypeDescription,
            Weight = Weight,
            Value = Value,
            Description = Description,
            TruckId = TruckId,
            VehicleOptionItems = VehicleOptionItems,
            IsWeighStationRequire = IsWeighStationrequire,
            MinimumOfferPrice = MinimumOfferPrice,
            MaximumOfferPrice = MaximumOfferPrice,
            VehicleQuantity = VehicleQuantity,
            IsSpecialOffer = IsSpecialOffer
        };
    }
}