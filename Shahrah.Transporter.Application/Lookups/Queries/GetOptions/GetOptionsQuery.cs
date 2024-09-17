using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetOptions;

public class GetOptionsQuery : IRequest<IEnumerable<OptionDto>>
{
}