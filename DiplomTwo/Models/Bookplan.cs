using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Bookplan
{
    public int Id { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public int PriorityId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Prioritylevel Priority { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
