using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class VoucherResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public double Quantity { get; set; }

        public int SalePercent { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }
        public Guid? StatusId { get; set; }
    }
}
