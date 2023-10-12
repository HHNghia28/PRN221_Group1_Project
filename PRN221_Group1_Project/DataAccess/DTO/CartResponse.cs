using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class CartResponse
    {
        public Guid Id { get; set; }

        public double Quantity { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int? SalePercent { get; set; }

        public string? Image { get; set; }

        public Guid? ProductId { get; set; }
    }
}
