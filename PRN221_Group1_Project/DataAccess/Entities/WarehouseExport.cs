using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class WarehouseExport
{
    public Guid Id { get; set; }

    public double Quantity { get; set; }

    public DateTime Date { get; set; }

    public Guid? ScheduleId { get; set; }

    public Guid? WarehouseId { get; set; }

    public virtual ICollection<Destruction> Destructions { get; set; } = new List<Destruction>();

    public virtual Schedule? Schedule { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
