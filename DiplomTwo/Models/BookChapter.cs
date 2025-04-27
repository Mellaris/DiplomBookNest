using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class BookChapter
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public int ChapterNumber { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? WordCount { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
