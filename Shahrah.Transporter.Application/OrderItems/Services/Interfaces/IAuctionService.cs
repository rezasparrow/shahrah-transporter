using Shahrah.Framework.Events;

namespace Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

public interface IAuctionService
{
    Task AuctionClosed(AuctionClosedEvent message);
}