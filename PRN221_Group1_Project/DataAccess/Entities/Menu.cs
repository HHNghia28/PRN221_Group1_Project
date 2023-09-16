using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Menu
{
    public Guid Id { get; set; }

    public double Quantity { get; set; }

    public Guid? ScheduleId { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
