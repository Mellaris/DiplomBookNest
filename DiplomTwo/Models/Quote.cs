using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Quote
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public int? BookId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Book? Book { get; set; }

    public virtual Reader? Reader { get; set; }
}
