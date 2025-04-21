using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Synopsis { get; set; }

    public string? Quote { get; set; }

    public string? Description { get; set; }

    public int? PageCount { get; set; }

    public int? AgeLimit { get; set; }

    public double? Rating { get; set; }

    public string? CoverImage { get; set; }

    public int? Kolread { get; set; }

    public int? Kolrev { get; set; }

    public int? Kolplan { get; set; }

    public DateTime? Dateadd { get; set; }

    public bool IsAuthorBook { get; set; }

    public int? SeriesId { get; set; }

    public virtual ICollection<AuthorBookCharacter> AuthorBookCharacters { get; set; } = new List<AuthorBookCharacter>();

    public virtual ICollection<AuthorBookWorld> AuthorBookWorlds { get; set; } = new List<AuthorBookWorld>();

    public virtual ICollection<BookChapter> BookChapters { get; set; } = new List<BookChapter>();

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    public virtual ICollection<Bookauthor> Bookauthors { get; set; } = new List<Bookauthor>();

    public virtual ICollection<Bookreview> Bookreviews { get; set; } = new List<Bookreview>();

    public virtual ICollection<Personallibrary> Personallibraries { get; set; } = new List<Personallibrary>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    public virtual ICollection<Readerfavoritebook> Readerfavoritebooks { get; set; } = new List<Readerfavoritebook>();

    public virtual ICollection<Readingplan> Readingplans { get; set; } = new List<Readingplan>();

    public virtual Series? Series { get; set; }

    public virtual ICollection<Seriesbook> Seriesbooks { get; set; } = new List<Seriesbook>();
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "PhotoForBook", CoverImage); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
    }
}
