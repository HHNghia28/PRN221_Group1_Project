using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Voucher
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public double Quantity { get; set; }

    public int SalePercent { get; set; }

    public string? Description { get; set; }

    public Guid? StatusId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual VoucherStatus? Status { get; set; }
}
