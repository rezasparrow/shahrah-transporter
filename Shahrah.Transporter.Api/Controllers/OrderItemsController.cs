using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.OrderItems.Commands.ConfirmLoading;
using Shahrah.Transporter.Application.OrderItems.Commands.EndTrip;
using Shahrah.Transporter.Application.OrderItems.Commands.OrderItemTechnicalyConfirmed;
using Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class OrderItemsController(IMediator mediator, ICurrentUserService currentUserService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPut("{orderItemId:int}/ConfirmLoading")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ConfirmLoading(int orderItemId)
    {
        await _mediator.Send(new ConfirmLoadingCommand(orderItemId, _currentUserService.Id));

        return Ok();
    }

    [HttpPut("{orderItemId:int}/TechnicalyConfirmed")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> TechnicalyConfirmed(int orderItemId)
    {
        await _mediator.Send(new OrderItemTechnicalyConfirmedCommand(orderItemId, _currentUserService.Id));

        return Ok();
    }

    [HttpPut("{orderItemId:int}/RegisterWaybill")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> RegisterWaybill(int orderItemId, [FromBody] RegisterWayBillRequestModel model)
    {
        await _mediator.Send(new RegisterWaybillCodeCommand(orderItemId, model.WaybillCode, _currentUserService.Id));

        return Ok();
    }

    [HttpPut("{orderItemId:int}/EndTrip")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> EndTrip(int orderItemId)
    {
        await _mediator.Send(new ConfirmTripEndedCommand(orderItemId, _currentUserService.Id));

        return Ok();
    }
}