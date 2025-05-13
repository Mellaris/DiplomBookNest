using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int ChapterId { get; set; }

    public int ReaderId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual BookChapter Chapter { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
