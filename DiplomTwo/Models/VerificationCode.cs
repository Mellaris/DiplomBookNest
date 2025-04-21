using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class VerificationCode
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
