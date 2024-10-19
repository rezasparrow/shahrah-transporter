using MediatR;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItemsByWallet;

/// <summary>
/// پرداخت لاین آیتمها از کیف پول
/// </summary>
public class PayByWalletCommand(long personId, List<int> orderItemIdentities) : IRequest
{
    public long PersonId { get; } = personId;
    public List<int> OrderItemIdentities { get; } = orderItemIdentities;
}