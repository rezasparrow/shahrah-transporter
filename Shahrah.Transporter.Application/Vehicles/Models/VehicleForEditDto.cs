using Shahrah.Transporter.Application.Lookups.Models;
using System;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Vehicles.Models;

public class VehicleForEditDto
{
    public int Id { get; set; }
    public PlateNumberDto PlateNumber { get; set; }
    public string Vin { get; set; }
    public string SmartCardNumber { get; set; }
    public DateTime SmartCardExpirationDate { get; set; }
    public int TruckTypeId { get; set; }
    public float NetLoadingCapacity { get; set; }
    public float GrossLoadingCapacity { get; set; }

    public ICollection<int> VehicleOptionItems { get; set; }
    public bool IsTransporterVehicleOwner { get; set; }

    public string OwnerFirstName { get; set; }
    public string OwnerLastName { get; set; }
    public string OwnerNationalCode { get; set; }
}