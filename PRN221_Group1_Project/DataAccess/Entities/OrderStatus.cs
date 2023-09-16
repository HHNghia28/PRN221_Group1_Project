using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class OrderStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
