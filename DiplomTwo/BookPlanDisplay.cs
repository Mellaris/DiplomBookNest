using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomTwo
{
    public class BookPlanDisplay
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int PriorityId { get; set; }
        public IBrush PriorityColor { get; set; }
    }
}
