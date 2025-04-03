using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class News
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ContentText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? Category { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
