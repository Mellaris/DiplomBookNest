using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo.Models;

public partial class Character
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageName { get; set; }

    public string? Description { get; set; }

    public string? Personality { get; set; }

    public string? Hobbies { get; set; }

    public string? Goal { get; set; }

    public int? Age { get; set; }

    public int GenderId { get; set; }

    public string? Ethnicity { get; set; }

    public string? Occupation { get; set; }

    public string? Fears { get; set; }

    public string? Strengths { get; set; }

    public string? Weaknesses { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "PhotosForCharacters", ImageName); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
    }
}
