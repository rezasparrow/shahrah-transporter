using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvince;

public class GetProvinceQuery : IRequest<ProvinceDto>
{
    public GetProvinceQuery(int provinceId)
    {
        ProvinceId = provinceId;
    }

    public int ProvinceId { get; }
}