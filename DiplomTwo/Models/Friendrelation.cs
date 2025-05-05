using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Friendrelation
{
    public int Id { get; set; }

    public int Fromuserid { get; set; }

    public int Touserid { get; set; }

    public int Statusid { get; set; }

    public DateTime? Requestdate { get; set; }

    public virtual User Fromuser { get; set; } = null!;

    public virtual Friendrequeststatus Status { get; set; } = null!;

    public virtual User Touser { get; set; } = null!;
}
