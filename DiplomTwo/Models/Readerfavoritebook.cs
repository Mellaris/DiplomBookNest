using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Readerfavoritebook
{
    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public int Id { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
