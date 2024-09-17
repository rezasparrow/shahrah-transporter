using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using System;

namespace Shahrah.Transporter.Application.Transporters.Models;

public class RegisterTransporterDto
{
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
    public City City { get; set; }
}