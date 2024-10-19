using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.Transporters.Commands.RegisterTransporter;
using Shahrah.Transporter.Application.Transporters.Models;
using Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class TransportersController(IMediator mediator, ICurrentUserService currentUserService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPost]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RegisterTransporter([FromBody] RegisterTransporterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var transporter = model.ToRegisterTransporterDto();
        var person = model.Manager.ToRegisterPersonDto(_currentUserService.Id, _currentUserService.MobileNumber);

        await _mediator.Send(new RegisterTransporterCommand(transporter, person));

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(TransporterDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TransporterInfo()
    {
        var transporter = await _mediator.Send(new GetTransporterQuery(_currentUserService.Id));
        return Ok(transporter);
    }
}