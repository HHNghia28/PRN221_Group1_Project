using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Shift
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
