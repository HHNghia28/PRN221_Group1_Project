using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class AccountStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
