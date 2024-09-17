using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class PersonOrder : Entity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public long PersonId { get; set; }
    public Person Person { get; set; }
    public decimal? OfferedPrice { get; set; }
}