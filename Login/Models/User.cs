using System;
using System.Collections.Generic;

namespace Login.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? Created { get; set; }
}
