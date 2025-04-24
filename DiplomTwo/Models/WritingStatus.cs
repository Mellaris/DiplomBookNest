using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class WritingStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ElectronicBooksInfo> ElectronicBooksInfos { get; set; } = new List<ElectronicBooksInfo>();
}
