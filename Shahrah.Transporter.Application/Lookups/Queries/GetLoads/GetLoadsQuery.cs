using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetLoads;

public class GetLoadsQuery : IRequest<IEnumerable<LoadDto>>
{
}