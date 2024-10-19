using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.People.Queries.GetCashBalance;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class CashBalancesController(IMediator mediator, ICurrentUserService currentUserService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<long>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCashBalance()
    {
        var cashBalance = await _mediator.Send(new GetCashBalanceQuery(_currentUserService.Id));
        return Ok(cashBalance);
    }
}