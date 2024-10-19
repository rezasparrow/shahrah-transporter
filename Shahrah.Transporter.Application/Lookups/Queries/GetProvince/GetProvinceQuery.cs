using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvince;

public class GetProvinceQuery(int provinceId) : IRequest<ProvinceDto>
{
    public int ProvinceId { get; } = provinceId;
}