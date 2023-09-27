using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class WarehouseImportRequest
    {

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid? WarehouseItemId { get; set; }
    }
}
