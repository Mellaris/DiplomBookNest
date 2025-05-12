using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class ElectronicBooksInfo
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public int? TargetWordCount { get; set; }

    public int? CurrentWordCount { get; set; }

    public int? EstimatedPageCount { get; set; }

    public int? ChapterCount { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? StatusId { get; set; }

    public int? AccessLevelId { get; set; }

    public virtual Publicaccesslevel? AccessLevel { get; set; }

    public virtual Book? Book { get; set; }

    public virtual WritingStatus? Status { get; set; }
}
