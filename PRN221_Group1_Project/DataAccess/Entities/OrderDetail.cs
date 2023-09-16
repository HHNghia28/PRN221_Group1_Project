using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public decimal Price { get; set; }

    public int? SalePercent { get; set; }

    public double Quantity { get; set; }

    public string? Note { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
