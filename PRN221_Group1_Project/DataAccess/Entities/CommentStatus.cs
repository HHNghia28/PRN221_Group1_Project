using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class CommentStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
