﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Categogy
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
