using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Friendrequeststatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Friendrelation> Friendrelations { get; set; } = new List<Friendrelation>();
}
