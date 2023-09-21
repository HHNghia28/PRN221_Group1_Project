﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class RecipeDTO
    {
        public Guid Id { get; set; }

        public double Quantity { get; set; }

        public string ProductName { get; set; }

        public string Unit { get; set; }

        public string WarehouseItemName { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? WarehouseItemId { get; set; }
    }
}
