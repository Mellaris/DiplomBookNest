using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Author
{
    public int Id { get; set; }

    public string? Biography { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public string Name { get; set; } = null!;
}
