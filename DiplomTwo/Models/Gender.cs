using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class Gender
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
