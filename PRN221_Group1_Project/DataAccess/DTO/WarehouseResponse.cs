using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class WarehouseResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Quantity { get; set; }

        public string? ItemName { get; set; }
        public Guid? WarehouseItemId { get; set; }
    }
}
