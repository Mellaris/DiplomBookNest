using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo.Models;

public partial class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Picturename { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "achievementsPhotos", Picturename); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
    }
}
