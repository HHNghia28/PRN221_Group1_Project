using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class WarehouseItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? CategogyItemId { get; set; }

    public Guid? UnitId { get; set; }

    public virtual CategogyWarehouseItem? CategogyItem { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Unit? Unit { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
