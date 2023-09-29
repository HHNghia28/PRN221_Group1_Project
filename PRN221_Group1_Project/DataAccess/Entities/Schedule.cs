using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Schedule
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Destruction> Destructions { get; set; } = new List<Destruction>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();

    public virtual ICollection<WarehouseExport> WarehouseExports { get; set; } = new List<WarehouseExport>();
}
