using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Seriesbook
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public int BookId { get; set; }

    public int? BookOrder { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Series Series { get; set; } = null!;
}
