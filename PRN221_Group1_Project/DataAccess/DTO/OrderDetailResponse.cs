using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; } = Guid.Empty;

        public decimal Price { get; set; }

        public int? SalePercent { get; set; }

        public double Quantity { get; set; }

        public string? Note { get; set; }

        public string? ProductName { get; set; }

        public string? Image { get; set; }
    }
}
