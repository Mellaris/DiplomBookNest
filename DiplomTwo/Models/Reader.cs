using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo.Models;

public partial class Reader
{
    public int IdReader { get; set; }

    public int? UserId { get; set; }

    public int? YearlyGoal { get; set; }

    public string? AvatarUrl { get; set; }

    public string? ProfileDescription { get; set; }

    public virtual Appauthor? Appauthor { get; set; }

    public virtual ICollection<Bookplan> Bookplans { get; set; } = new List<Bookplan>();

    public virtual ICollection<Bookreview> Bookreviews { get; set; } = new List<Bookreview>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Personallibrary> Personallibraries { get; set; } = new List<Personallibrary>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    public virtual ICollection<Readerfavoritebook> Readerfavoritebooks { get; set; } = new List<Readerfavoritebook>();

    public virtual User? User { get; set; }

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "AvatarPhoto", AvatarUrl); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
    }
}
