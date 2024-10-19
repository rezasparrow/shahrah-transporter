using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetOptions;

public class GetOptionsQuery : IRequest<IEnumerable<OptionDto>>
{
}