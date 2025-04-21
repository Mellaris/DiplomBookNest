using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Picturename { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();
}
