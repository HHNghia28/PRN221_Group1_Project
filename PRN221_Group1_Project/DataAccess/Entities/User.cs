using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string AddressDetail { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool? DefaultUser { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? StatusId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual AccountStatus? Status { get; set; }
}
