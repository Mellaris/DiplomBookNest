using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Userachievement
{
    public int Id { get; set; }

    public int ReaderId { get; set; }

    public int AchievementId { get; set; }

    public DateTime? EarnedAt { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
