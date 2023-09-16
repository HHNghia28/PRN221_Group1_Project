using System;
using System.Collections.Generic;

namespace G1FOODLibrary.Entities;

public partial class Comment
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public Guid? StatusId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Product? Product { get; set; }

    public virtual CommentStatus? Status { get; set; }
}
