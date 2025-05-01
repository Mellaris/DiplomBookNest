using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class WorldSection
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public virtual ICollection<WorldSectionContent> WorldSectionContents { get; set; } = new List<WorldSectionContent>();
}
