using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class WarehouseImport
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public double Quantity { get; set; }

    public decimal Price { get; set; }

    public Guid? WarehouseId { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
