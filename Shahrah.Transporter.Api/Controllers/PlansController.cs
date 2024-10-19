using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetPlans;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class PlansController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var Plans = await _mediator.Send(new GetPlansQuery());
        return Ok(Plans);
    }
}