using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Author
{
    public int Id { get; set; }

    public string? Biography { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? DeathDate { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Bookauthor> Bookauthors { get; set; } = new List<Bookauthor>();
}
