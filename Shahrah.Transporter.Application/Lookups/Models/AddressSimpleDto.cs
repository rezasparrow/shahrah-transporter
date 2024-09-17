namespace Shahrah.Transporter.Domain.Models.DataTransferObjects;

public class AddressSimpleDto
{
    public int CityId { get; set; }
    public string CityName { get; set; }
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
}