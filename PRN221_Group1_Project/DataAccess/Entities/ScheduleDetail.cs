using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class ScheduleDetail
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ScheduleId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
