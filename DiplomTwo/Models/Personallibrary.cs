using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Personallibrary
{
    public int Id { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public double? Rating { get; set; }

    public int? PlotRating { get; set; }

    public int? CharactersRating { get; set; }

    public int? WorldRating { get; set; }

    public int? RomanceRating { get; set; }

    public int? HumorRating { get; set; }

    public int? MeaningRating { get; set; }

    public DateTime? DateAdd { get; set; }

    public string? Feedback { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
