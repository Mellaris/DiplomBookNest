using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Prioritylevel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<Bookplan> Bookplans { get; set; } = new List<Bookplan>();
}
