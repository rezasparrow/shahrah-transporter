using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Transporters.Models;

public class TransporterDto(Domain.Entities.Transporter item)
{
    public string Name { get; set; } = item.Name;
    public string NationalId { get; set; } = item.NationalId;
    public string LicenseSerialNumber { get; set; } = item.LicenseSerialNumber;
    public DateTime LicenseExpirationDate { get; set; } = item.LicenseExpirationDate;
    public string Address { get; set; } = item.Address;
    public string PostalCode { get; set; } = item.PostalCode;
    public string PhoneNumber { get; set; } = item.PhoneNumber;
    public TransporterActivityZoneType TransporterActivityZone { get; set; } = item.ActivityZone;
    public double Latitude { get; set; } = item.Latitude;
    public double Longitude { get; set; } = item.Longitude;
    public int CityId { get; set; } = item.CityId;
    public int ProvinceId { get; set; } = item.City.ProvinceId;
}