using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string EncryptedPassword { get; set; } = null!;

    public Guid? RoleId { get; set; }

    public Guid? StatusId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Destruction> Destructions { get; set; } = new List<Destruction>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();

    public virtual AccountStatus? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<WarehouseImport> WarehouseImports { get; set; } = new List<WarehouseImport>();
}
