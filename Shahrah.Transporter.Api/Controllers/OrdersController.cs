using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Models;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.OrderItems.Models;
using Shahrah.Transporter.Application.OrderItems.Queries.GetCanceledOrderItems;
using Shahrah.Transporter.Application.OrderItems.Queries.GetOrderItems;
using Shahrah.Transporter.Application.OrderItems.Queries.GetPaidOrderItems;
using Shahrah.Transporter.Application.OrderItems.Queries.GetPendingPaymentOrderItems;
using Shahrah.Transporter.Application.OrderItems.Queries.GetTripEndedOrderItems;
using Shahrah.Transporter.Application.Orders.Commands.AssignDriverToOrder;
using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using Shahrah.Transporter.Application.Orders.Commands.FindDriver;
using Shahrah.Transporter.Application.Orders.Commands.HideOrder;
using Shahrah.Transporter.Application.Orders.Commands.OfferPrice;
using Shahrah.Transporter.Application.Orders.Commands.PendOrder;
using Shahrah.Transporter.Application.Orders.Commands.RegisterOrder;
using Shahrah.Transporter.Application.Orders.Commands.ReSendOrder;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Application.Orders.Queries.GetOrder;
using Shahrah.Transporter.Application.Orders.Queries.GetOrders;
using Shahrah.Transporter.Application.Orders.Queries.GetOrderSenderInfo;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class OrdersController(IMediator mediator, ICurrentUserService currentUserService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> RegisterOrder([FromBody] OrderModel viewModel)
    {
        //ToDo : Get LoadReceiver data (FirstName,LastName ,NationalCode,MobileNumber ,IsCompany,
        //                              CompanyName ,CompanyNationalId  )

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = viewModel.ToRegisterOrderDto();

        await _mediator.Send(new RegisterOrderCommand(order, _currentUserService.Id));

        return Ok();
    }

    [HttpPost("{orderId:int}/ReSend")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ReSendOrder(int orderId, [FromBody] OrderReSendModel viewModel)
    {
        await _mediator.Send(new ReSendOrderCommand(_currentUserService.Id, orderId, viewModel.MinimumOfferPrice, viewModel.MaximumOfferPrice, viewModel.VehicleQuantity));

        return Ok();
    }

    [HttpPost("{orderId:int}/FindDriver")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> FindDriver(int orderId, [FromBody] FindDriverModel viewModel)
    {
        await _mediator.Send(new FindDriverCommand(_currentUserService.Id, orderId, viewModel.MinimumOfferPrice, viewModel.MaximumOfferPrice, viewModel.VehicleQuantity));
        return Ok();
    }

    [HttpPatch("{orderId:int}/driver")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AllocateDriver(int orderId, [FromBody] AllocateDriverModel viewModel)
    {
        await _mediator.Send(new AssignDriverToOrderCommand(_currentUserService.Id, orderId, viewModel.VehicleId, viewModel.Price));
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var orders = (await _mediator.Send(new GetOrdersQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<OrderDto>(orders, orders.Count));
    }

    [HttpGet("{orderId:int}/sender")]
    [ProducesResponseType(typeof(IEnumerable<OrderSenderInfoDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSenderInfo(int orderId)
    {
        var info = await _mediator.Send(new GetOrderSenderInfoQuery(_currentUserService.Id, orderId));
        return Ok(info);
    }

    [HttpPut("{id:int}/Hide")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> HideOrder(int id)
    {
        await _mediator.Send(new HideOrderCommand(id, _currentUserService.Id));

        return Ok();
    }

    [HttpPost("{orderId:int}/OfferPrice")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> OfferPrice(int orderId, [FromBody] decimal price)
    {
        await _mediator.Send(new OfferPriceCommand(_currentUserService.Id, orderId, price));

        return Ok();
    }

    [HttpPut("{orderId:int}/Pend")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> PendOrder(int orderId)
    {
        await _mediator.Send(new PendOrderCommand(orderId, _currentUserService.Id));

        return Ok();
    }

    [HttpPut("{orderId:int}/Close")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CloseOrder(int orderId)
    {
        await _mediator.Send(new CloseOrderCommand(CloseOrderTypeEnum.ForceClose, orderId, _currentUserService.Id));

        return Ok();
    }

    [HttpGet("Items")]
    [ProducesResponseType(typeof(IEnumerable<OrderItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrdersOrderItems()
    {
        var orders = (await _mediator.Send(new GetPendingPaymentOrderItemsQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<OrderItemDto>(orders, orders.Count));
    }

    [HttpGet("Paid")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPaidOrder()
    {
        var orders = (await _mediator.Send(new GetPaidOrderItemsQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<PaidOrderItemDto>(orders, orders.Count));
    }

    [HttpGet("{orderId:int}/Items")]
    [ProducesResponseType(typeof(IEnumerable<OrderItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItems(int orderId)
    {
        var orders = (await _mediator.Send(new GetOrderItemsQuery(orderId, _currentUserService.Id))).ToList();
        return Ok(new CountableList<OrderItemDto>(orders, orders.Count));
    }

    [HttpGet("{orderId:int}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(int orderId)
    {
        var order = await _mediator.Send(new GetOrderQuery(_currentUserService.Id, orderId));
        return Ok(order);
    }

    [HttpGet("Canceled")]
    [ProducesResponseType(typeof(IEnumerable<CanceledOrderItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCanceledOrder()
    {
        var orders = (await _mediator.Send(new GetCanceledOrderItemsQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<CanceledOrderItemDto>(orders, orders.Count));
    }

    [HttpGet("TripEnded")]
    [ProducesResponseType(typeof(IEnumerable<TripEndedOrderItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTripEndedOrder()
    {
        var orders = (await _mediator.Send(new GetTripEndedOrderItemsQuery(_currentUserService.Id))).ToList();
        return Ok(new CountableList<TripEndedOrderItemDto>(orders, orders.Count));
    }
}