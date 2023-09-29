using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class WarehouseExportResponse
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Quantity { get; set; }

        public string? WarehouseItem { get; set; }
    }
}
