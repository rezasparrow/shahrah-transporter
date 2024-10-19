using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItems;
using Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItemsByWallet;
using Shahrah.Transporter.Application.Payments.Commands.ChargeWallet;
using Shahrah.Transporter.Application.Payments.Commands.RegisterPaymentForSubscription;
using Shahrah.Transporter.Application.Payments.Commands.VerifyPayment;

namespace Shahrah.Transporter.Api.Controllers;

public class PaymentsController(
    IOnlinePayment onlinePayment,
    IMediator mediator,

    IOptions<AppSettings> settings,
    ICurrentUserService currentUserService) : BaseController
{
    private readonly IOnlinePayment _onlinePayment = onlinePayment;
    private readonly IMediator _mediator = mediator;

    private readonly AppSettings _settings = settings.Value;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayOrderItemsModel payOrderItemsModel)
    {
        var result = await _mediator.Send(new PayOrderItemsCommand(payOrderItemsModel.SelectedGateway, payOrderItemsModel.OrderItemIds));
        return Ok(new
        {
            result.IsSucceed,
            result.Message,
            Transporter = CreateTransporterForClientApp(result.GatewayTransporter)
        });
    }

    [Route("verify")]
    [AllowAnonymous]
    [HttpGet, HttpPost]
    public async Task<IActionResult> Verify()
    {
        var invoice = await _onlinePayment.FetchAsync();
        var verifyResult = await _mediator.Send(new VerifyPaymentCommand(invoice));

        return Redirect($"{_settings.ClientAppUrl}?IsSuccess={verifyResult?.IsSucceed ?? false}&Message={verifyResult?.Message ?? string.Empty}&TrackingNumber={verifyResult?.TrackingNumber ?? 0}");
    }

    [HttpPost("PayByWallet")]
    public async Task<IActionResult> PayByWallet([FromBody] PayByWalletModel viewModel)
    {
        await _mediator.Send(new PayByWalletCommand(_currentUserService.Id, viewModel.OrderItemIdentitites));
        return Ok();
    }

    [HttpGet("gateways")]
    public IActionResult GetGateways()
    {
        var gateways = Enum.GetValues<GatewayType>().Select(gateway => new
        {
            Name = gateway.ToString(),
            Value = gateway.GetHashCode()
        });

        return Ok(gateways);
    }

    private static object CreateTransporterForClientApp(IGatewayTransporter gatewayTransporter)
    {
        if (gatewayTransporter?.Descriptor == null) return null;

        var form = gatewayTransporter.Descriptor.Form?.Select(item => new
        {
            item.Key,
            item.Value
        });

        return new
        {
            gatewayTransporter.Descriptor.Type,
            gatewayTransporter.Descriptor.Url,
            Form = form
        };
    }

    [HttpPost("cashBalance")]
    public async Task<IActionResult> CashBalance([FromBody] PayCashBalanceModel viewModel)
    {
        var result = await _mediator.Send(new ChargeWalletCommand(viewModel.SelectedGateway, viewModel.Amount, _currentUserService.Id));

        return Ok(new
        {
            result.IsSucceed,
            result.Message,
            Transporter = CreateTransporterForClientApp(result.GatewayTransporter)
        });
    }

    [HttpPost("subscription")]
    public async Task<IActionResult> AddSubscription([FromBody] PaySubscriptionModel viewModel)
    {
        var command = new RegisterPaymentForSubscriptionCommand(viewModel.SelectedGateway,
                                                                viewModel.PlanId, _currentUserService.Id);
        var result = await _mediator.Send(command);

        return Ok(new
        {
            result.IsSucceed,
            result.Message,
            Transporter = CreateTransporterForClientApp(result.GatewayTransporter)
        });
    }
}