using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPlans;

public class GetPlansQuery : IRequest<IEnumerable<PlanDto>>
{
}