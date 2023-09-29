using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class WarehouseExportRequest
    {
        public double Quantity { get; set; }

        public Guid? WarehouseItemId { get; set; }
    }
}
