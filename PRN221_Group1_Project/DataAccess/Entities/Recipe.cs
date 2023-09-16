using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Recipe
{
    public Guid Id { get; set; }

    public double Quantity { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? WarehouseItemId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual WarehouseItem? WarehouseItem { get; set; }
}
