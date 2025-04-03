using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Priority
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Readingplan> Readingplans { get; set; } = new List<Readingplan>();
}
