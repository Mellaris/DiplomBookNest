using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class AuthorBookWorld
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public string Section { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual Book? Book { get; set; }
}
