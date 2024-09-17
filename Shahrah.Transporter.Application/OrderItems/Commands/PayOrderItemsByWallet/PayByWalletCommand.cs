using MediatR;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItemsByWallet;

/// <summary>
/// پرداخت لاین آیتمها از کیف پول
/// </summary>
public class PayByWalletCommand : IRequest
{
    public long PersonId { get; }
    public List<int> OrderItemIdentities { get; }

    public PayByWalletCommand(long personId, List<int> orderItemIdentities)
    {
        OrderItemIdentities = orderItemIdentities;
        PersonId = personId;
    }
}