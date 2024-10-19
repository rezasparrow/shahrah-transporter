using MediatR;

namespace Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;

public class GetDriversQuery(string nationalCode) : IRequest<Framework.Models.Driver>
{
    public string NationalCode { get; set; } = nationalCode;
}