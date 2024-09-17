namespace Shahrah.Transporter.Domain.Models.DataTransferObjects;

public class AddressDto
{
    public int CityId { get; set; }
    public string CityName { get; set; }
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}