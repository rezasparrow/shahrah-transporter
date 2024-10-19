using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;
using PersonTypeEnum = Shahrah.Transporter.Domain.Enums.PersonTypeEnum;

namespace Shahrah.Transporter.Domain.Entities;

public class Person : Entity<long>
{
    public new long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public PersonStatus Status { get; set; }
    public AgentRegistrationStatus AgentRegistrationStatus { get; set; }
    public required string MobileNumber { get; set; }
    public string? LicenseSerialNumber { get; set; }
    public PersonTypeEnum PersonType { get; set; }
    public Transporter Transporter { get; set; }
    public long TransporterId { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<PersonOrder> ReceivedOrders { get; set; } = [];
    public ICollection<FinancialTransaction> FinancialTransactions { get; set; } = [];
}