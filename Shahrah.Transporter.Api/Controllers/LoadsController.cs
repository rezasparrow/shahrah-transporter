using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetLoads;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class LoadsController : BaseController
{
    private readonly IMediator _mediator;

    public LoadsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LoadDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var loads = await _mediator.Send(new GetLoadsQuery());
        return Ok(loads);
    }
}