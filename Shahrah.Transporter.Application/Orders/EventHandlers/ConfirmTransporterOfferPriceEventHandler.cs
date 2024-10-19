using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.EventHandlers;

/// <summary>
/// اگر قیمت بالاتر از قیمت پیشنهادی سندر باشه، سندر باید تائید بده
/// تو این ایونت میگه که سندر تائید داده یا نه
/// </summary>
public class ConfirmTransporterOfferPriceEventHandler(IOrderService orderService) : KafkaEventHandler<ConfirmTransporterOfferPriceEvent>
{
    private readonly IOrderService _orderService = orderService;

    public override async Task Handle(ConfirmTransporterOfferPriceEvent message)
    {
        await _orderService.ConfirmTransporterOfferPriceBySender(message);
    }
}