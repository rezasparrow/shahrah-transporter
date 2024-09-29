using Shahrah.Framework.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class VehicleModel
{
    [Display(Name = "PlateNumber", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public PlateNumberModel PlateNumber { get; set; }

    [Display(Name = "VIN", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string VIN { get; set; }

    [Display(Name = "SmartCardNumber", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string SmartCardNumber { get; set; }

    [Display(Name = "SmartCardExpirationDate", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public DateTime SmartCardExpirationDate { get; set; }

    [Display(Name = "TruckType", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int TruckTypeId { get; set; }

    [Display(Name = "NetLoadingCapacity", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public float NetLoadingCapacity { get; set; }

    [Display(Name = "GrossLoadingCapacity", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public float GrossLoadingCapacity { get; set; }

    public List<int> VehicleOptionItems { get; set; } = [];

    [Display(Name = "VehicleOwner", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public bool IsTransporterVehicleOwner { get; set; }

    [Display(Name = "OwnerFirstName", ResourceType = typeof(DisplayNameResource))]
    [StringLength(64, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string? OwnerFirstName { get; set; }

    [Display(Name = "OwnerLastName", ResourceType = typeof(DisplayNameResource))]
    [StringLength(128, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string? OwnerLastName { get; set; }

    [Display(Name = "OwnerNationalCode", ResourceType = typeof(DisplayNameResource))]
    [StringLength(11, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string? OwnerNationalCode { get; set; }
}
