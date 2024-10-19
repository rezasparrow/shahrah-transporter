using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetProvinces;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class ProvincesController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var provinces = await _mediator.Send(new GetProvincesQuery());
        return Ok(provinces);
    }
}