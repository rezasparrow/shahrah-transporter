using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Models;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.People.Commands.AgentAccept;
using Shahrah.Transporter.Application.People.Commands.AgentAdd;
using Shahrah.Transporter.Application.People.Commands.AgentEdit;
using Shahrah.Transporter.Application.People.Commands.AgentNotAccept;
using Shahrah.Transporter.Application.People.Commands.DeleteAgent;
using Shahrah.Transporter.Application.People.Models;
using Shahrah.Transporter.Application.People.Queries.GetAgent;
using Shahrah.Transporter.Application.People.Queries.GetAgents;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class AgentsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AgentsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var agents = (await _mediator.Send(new GetAgentsQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<PersonDto>(agents, agents.Count));
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddAgent([FromBody] AgentAddModel viewModel)
    {
        await _mediator.Send(new AgentAddCommand
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            NationalCode = viewModel.NationalCode,
            MobileNumber = viewModel.MobileNumber,
            PersonId = _currentUserService.Id
        });

        return Ok();
    }

    [HttpPut("{agentId:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> EditAgent(int agentId, [FromBody] AgentEditModel viewModel)
    {
        await _mediator.Send(new AgentEditCommand
        {
            AgentId = agentId,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            NationalCode = viewModel.NationalCode,
            PersonId = _currentUserService.Id
        });

        return Ok();
    }

    [HttpDelete("{agentId:long}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteAgent(long agentId)
    {
        await _mediator.Send(new DeleteAgentCommand
        {
            AgentId = agentId,
            PersonId = _currentUserService.Id
        });

        return Ok();
    }

    [HttpGet("current")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLoggedInAgent()
    {
        return Ok(await _mediator.Send(new GetAgentQuery(_currentUserService.Id)));
    }

    [HttpPost("accept")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AgentAccept([FromBody] AgentAcceptModel acceptModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _mediator
            .Send(
                new AgentAcceptCommand(_currentUserService.Id,
                    acceptModel.BirthDate,
                    acceptModel.FirstName,
                    acceptModel.LastName,
                    acceptModel.NationalCode
                ));
        return Ok();
    }

    [HttpPost("notAccept")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AgentNotAccept()
    {
        await _mediator.Send(new AgentNotAcceptCommand(_currentUserService.Id));
        return Ok();
    }
}