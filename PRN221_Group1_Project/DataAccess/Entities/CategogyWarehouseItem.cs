using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class CategogyWarehouseItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<WarehouseItem> WarehouseItems { get; set; } = new List<WarehouseItem>();
}
