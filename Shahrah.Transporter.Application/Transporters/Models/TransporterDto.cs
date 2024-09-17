using Shahrah.Transporter.Domain.Enums;
using System;

namespace Shahrah.Transporter.Application.Transporters.Models;

public class TransporterDto
{
    public TransporterDto(Domain.Entities.Transporter item)
    {
        Name = item.Name;
        NationalId = item.NationalId;
        LicenseSerialNumber = item.LicenseSerialNumber;
        LicenseExpirationDate = item.LicenseExpirationDate;
        Address = item.Address;
        PostalCode = item.PostalCode;
        PhoneNumber = item.PhoneNumber;
        TransporterActivityZone = item.ActivityZone;
        Latitude = item.Latitude;
        Longitude = item.Longitude;
        CityId = item.CityId;
        ProvinceId = item.City.ProvinceId;
    }

    public string Name { get; set; }
    public string NationalId { get; set; }
    public string LicenseSerialNumber { get; set; }
    public DateTime LicenseExpirationDate { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public TransporterActivityZoneType TransporterActivityZone { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CityId { get; set; }
    public int ProvinceId { get; set; }
}