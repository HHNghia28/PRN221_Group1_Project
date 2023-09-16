using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Cart
{
    public Guid Id { get; set; }

    public double Quantity { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Product? Product { get; set; }
}
