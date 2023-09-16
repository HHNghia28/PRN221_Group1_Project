using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Warehouse
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Quantity { get; set; }

    public Guid? WarehouseItemId { get; set; }

    public virtual ICollection<WarehouseExport> WarehouseExports { get; set; } = new List<WarehouseExport>();

    public virtual ICollection<WarehouseImport> WarehouseImports { get; set; } = new List<WarehouseImport>();

    public virtual WarehouseItem? WarehouseItem { get; set; }
}
