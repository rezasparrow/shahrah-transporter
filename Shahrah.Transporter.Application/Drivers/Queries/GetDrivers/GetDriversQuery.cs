using MediatR;

namespace Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;

public class GetDriversQuery : IRequest<Framework.Models.Driver>
{
    public GetDriversQuery(string nationalCode)
    {
        NationalCode = nationalCode;
    }

    public string NationalCode { get; set; }
}