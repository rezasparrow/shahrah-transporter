using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Domain.Entities;

public class Subscription : Entity<int>
{
    public Subscription()
    { }

    public Subscription(int id) : base(id)
    {
    }

    public long? PersonId { get; set; }
    public Person Person { get; set; }
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
    public DateTime ExpirationDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public Payment Payment { get; set; }
}