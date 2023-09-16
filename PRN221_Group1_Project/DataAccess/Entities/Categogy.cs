using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Categogy
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
