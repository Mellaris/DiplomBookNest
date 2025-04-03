using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Readingplan
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public int? BookId { get; set; }

    public int? PriorityId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Priority? Priority { get; set; }

    public virtual Reader? Reader { get; set; }
}
