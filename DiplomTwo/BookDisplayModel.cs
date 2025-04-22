using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomTwo
{
    public class BookDisplayModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;

        public string? Synopsis { get; set; }

        public string? Quote { get; set; }
        public Bitmap CoverBitmap { get; set; }
        public double? Rating { get; set; }

        public int? PlotRating { get; set; }

        public int? CharactersRating { get; set; }

        public int? WorldRating { get; set; }

        public int? RomanceRating { get; set; }

        public int? HumorRating { get; set; }

        public int? MeaningRating { get; set; }
        public string AuthorName {  get; set; }
    }
}
