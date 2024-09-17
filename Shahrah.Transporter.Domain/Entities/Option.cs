using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Option : Entity<int>
{
    public Option(int id) : base(id)
    {
    }

    public string Title { get; set; }
    public OptionType Type { get; set; }
    public ICollection<OptionItem> Items { get; set; }
}