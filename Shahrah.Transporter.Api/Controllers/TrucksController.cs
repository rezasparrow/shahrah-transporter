using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetOptions;
using Shahrah.Transporter.Application.Lookups.Queries.GetTrucks;
using Shahrah.Transporter.Application.Lookups.Queries.GetTruksByLoadWeight;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class TrucksController : BaseController
{
    private readonly IMediator _mediator;

    public TrucksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TruckDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var trucks = await _mediator.Send(new GetTrucksQuery());
        return Ok(trucks);
    }

    [HttpGet("LoadWeight/{weight:double}")]
    [ProducesResponseType(typeof(IEnumerable<TruckDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTruksByLoadWeight(double weight)
    {
        var trucks = await _mediator.Send(new GetTruksByLoadWeightQuery(weight));
        return Ok(trucks);
    }

    [HttpGet("features")]
    [ProducesResponseType(typeof(IEnumerable<OptionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Features()
    {
        var trucks = await _mediator.Send(new GetOptionsQuery());
        return Ok(trucks);
    }
}