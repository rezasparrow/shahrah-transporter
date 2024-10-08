﻿using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Commands.ReSendOrder;

internal class ReSendOrderCommandHandler : IRequestHandler<ReSendOrderCommand, Unit>
{
    private readonly IOrderService _orderService;

    public ReSendOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(ReSendOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderService.ReSendOrder(request.PersonId, request.OrderId, request.MinimumOfferPrice,
            request.MaximumOfferPrice, request.VehicleQuantity, cancellationToken);

        return Unit.Value;
    }
}