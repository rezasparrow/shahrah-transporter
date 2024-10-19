using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// آکشن بسته میشه
/// </summary>
public class AuctionClosedEventHandler(IAuctionService auctionService) : KafkaEventHandler<AuctionClosedEvent>
{
    private readonly IAuctionService _auctionService = auctionService;

    public override async Task Handle(AuctionClosedEvent message)
    {
        await _auctionService.AuctionClosed(message);
    }
}