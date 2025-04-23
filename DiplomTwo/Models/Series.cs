using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Series
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Seriesbook> Seriesbooks { get; set; } = new List<Seriesbook>();

    public virtual User? User { get; set; }
}
