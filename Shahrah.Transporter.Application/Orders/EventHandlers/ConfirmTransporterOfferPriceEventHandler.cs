using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.EventHandlers;

/// <summary>
/// اگر قیمت بالاتر از قیمت پیشنهادی سندر باشه، سندر باید تائید بده
/// تو این ایونت میگه که سندر تائید داده یا نه
/// </summary>
public class ConfirmTransporterOfferPriceEventHandler : KafkaEventHandler<ConfirmTransporterOfferPriceEvent>
{
    private readonly IOrderService _orderService;

    public ConfirmTransporterOfferPriceEventHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override async Task Handle(ConfirmTransporterOfferPriceEvent message)
    {
        await _orderService.ConfirmTransporterOfferPriceBySender(message);
    }
}