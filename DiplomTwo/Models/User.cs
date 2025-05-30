﻿using System;
using System.Collections.Generic;

namespace DiplomTwo.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Userlastname { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? GenderId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Friendrelation> FriendrelationFromusers { get; set; } = new List<Friendrelation>();

    public virtual ICollection<Friendrelation> FriendrelationTousers { get; set; } = new List<Friendrelation>();

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

    public virtual ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
}
