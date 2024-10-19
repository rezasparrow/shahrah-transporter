using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Domain.Entities;

public class FinancialTransaction : Entity<int>
{
    public long PersonId { get; set; }
    public Person Person { get; set; }
    public decimal Amount { get; set; }
    public FinancialTransactionType TransactionType { get; set; }
    public Guid? RefrenceId { get; set; }
    public string Description { get; set; }
}