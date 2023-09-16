using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public Guid? StatusId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ScheduleId { get; set; }

    public Guid? VoucherId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Schedule? Schedule { get; set; }

    public virtual OrderStatus? Status { get; set; }

    public virtual User? User { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
