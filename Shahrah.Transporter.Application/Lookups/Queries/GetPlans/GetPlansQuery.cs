using MediatR;
using Shahrah.Transporter.Application.Lookups.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetPlans;

public class GetPlansQuery : IRequest<IEnumerable<PlanDto>>
{
}