using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPackages;

public class GetPackagesQuery : IRequest<IEnumerable<PackageDto>>
{
}