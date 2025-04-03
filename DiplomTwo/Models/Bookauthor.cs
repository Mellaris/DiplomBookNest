using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Bookauthor
{
    public int BookId { get; set; }

    public int? AuthorId { get; set; }

    public int? AppAuthorId { get; set; }

    public virtual Appauthor? AppAuthor { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Book Book { get; set; } = null!;
}
