using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Domain.Entities;

public class Option(int id) : Entity<int>(id)
{
    public string Title { get; set; }
    public OptionType Type { get; set; }
    public ICollection<OptionItem> Items { get; set; }
}