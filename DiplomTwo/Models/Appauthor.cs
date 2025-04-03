using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Appauthor
{
    public int Id { get; set; }

    public string? ProfileDescription { get; set; }

    public int? WrittenBooksCount { get; set; }

    public string? AvatarUrl { get; set; }

    public int? WritingGoal { get; set; }

    public virtual Reader IdNavigation { get; set; } = null!;
}
