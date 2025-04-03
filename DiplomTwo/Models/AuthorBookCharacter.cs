using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class AuthorBookCharacter
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Personality { get; set; }

    public virtual Book? Book { get; set; }
}
