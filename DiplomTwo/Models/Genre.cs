using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
