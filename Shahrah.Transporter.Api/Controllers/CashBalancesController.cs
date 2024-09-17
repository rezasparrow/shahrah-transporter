using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.People.Queries.GetCashBalance;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class CashBalancesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public CashBalancesController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<long>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCashBalance()
    {
        var cashBalance = await _mediator.Send(new GetCashBalanceQuery(_currentUserService.Id));
        return Ok(cashBalance);
    }
}