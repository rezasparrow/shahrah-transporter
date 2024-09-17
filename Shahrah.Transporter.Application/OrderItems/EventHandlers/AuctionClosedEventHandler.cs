using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// آکشن بسته میشه
/// </summary>
public class AuctionClosedEventHandler : KafkaEventHandler<AuctionClosedEvent>
{
    private readonly IAuctionService _auctionService;

    public AuctionClosedEventHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    public override async Task Handle(AuctionClosedEvent message)
    {
        await _auctionService.AuctionClosed(message);
    }
}