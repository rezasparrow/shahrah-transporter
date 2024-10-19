using MediatR;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Commands.OfferPrice;

public class OfferPriceCommandHandler(IOrderPricingService orderPricingService) : IRequestHandler<OfferPriceCommand, Unit>
{
    private readonly IOrderPricingService _orderPricingService = orderPricingService;

    public async Task<Unit> Handle(OfferPriceCommand request, CancellationToken cancellationToken)
    {
        await _orderPricingService.RegisterOfferPrice(request.OrderId, request.PersonId, request.Price, cancellationToken);
        return Unit.Value;
    }
}