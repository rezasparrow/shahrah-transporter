using Shahrah.Framework.Events;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

public interface IAuctionService
{
    Task AuctionClosed(AuctionClosedEvent message);
}