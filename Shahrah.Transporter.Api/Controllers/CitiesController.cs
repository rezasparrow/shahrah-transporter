using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetCities;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class CitiesController : BaseController
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(int provinceId)
    {
        var cities = await _mediator.Send(new GetCitiesQuery(provinceId));
        return Ok(cities);
    }
}