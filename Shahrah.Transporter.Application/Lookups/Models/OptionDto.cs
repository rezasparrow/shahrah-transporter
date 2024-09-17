using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Lookups.Models;

public class OptionDto
{
    public string Title { get; set; }
    public OptionTypeDto Type { get; set; }
    public ICollection<OptionItemDto> Items { get; set; }
}

public class OptionItemDto
{
    public int Id { get; set; }
    public string Value { get; set; }
}

public enum OptionTypeDto
{
    Single = 1,
    Multiple = 2
}