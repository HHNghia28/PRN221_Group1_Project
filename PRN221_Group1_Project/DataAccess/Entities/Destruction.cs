using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Destruction
{
    public Guid Id { get; set; }

    public double Quantity { get; set; }

    public string? Image { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ScheduleId { get; set; }

    public Guid? WarehouseExportId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Schedule? Schedule { get; set; }

    public virtual WarehouseExport? WarehouseExport { get; set; }
}
