using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Bookreview
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public int? BookId { get; set; }

    public string ReviewText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Reader? Reader { get; set; }
}
