using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Lookups.Queries.GetPackages;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class PackagesController : BaseController
{
    private readonly IMediator _mediator;

    public PackagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PackageDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var packages = await _mediator.Send(new GetPackagesQuery());
        return Ok(packages);
    }
}