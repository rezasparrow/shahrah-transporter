using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvinces;

public class GetProvincesQuery : IRequest<IEnumerable<ProvinceDto>>
{
}