using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvinces;

public class GetProvincesQuery : IRequest<IEnumerable<ProvinceDto>>
{
}