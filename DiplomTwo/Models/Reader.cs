using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Reader
{
    public int IdReader { get; set; }

    public int UserId { get; set; }

    public int? YearlyGoal { get; set; }

    public string? AvatarUrl { get; set; }

    public string? ProfileDescription { get; set; }

    public virtual Appauthor? Appauthor { get; set; }

    public virtual ICollection<Bookreview> Bookreviews { get; set; } = new List<Bookreview>();

    public virtual ICollection<Personallibrary> Personallibraries { get; set; } = new List<Personallibrary>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    public virtual ICollection<Readingplan> Readingplans { get; set; } = new List<Readingplan>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
