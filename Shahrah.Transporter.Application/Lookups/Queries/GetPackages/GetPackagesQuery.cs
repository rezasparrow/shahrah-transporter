using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPackages;

public class GetPackagesQuery : IRequest<IEnumerable<PackageDto>>
{
}