using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Payment : Entity<int>
{
    public long TrackingNumber { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public string TransactionCode { get; set; }
    public string GatewayName { get; set; }
    public string GatewayAccountName { get; set; }
    public string Message { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public long? PersonId { get; set; }
    public Person Person { get; set; }
    public int? SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
}