using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class WorldSectionContent
{
    public int Id { get; set; }

    public int WorldSectionId { get; set; }

    public int BookId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual WorldSection WorldSection { get; set; } = null!;
}
