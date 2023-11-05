using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public Guid? UserID { get; set; }
        public string? Username { get; set; }

        public int SalePercent { get; set; }

        public string? VoucherCode { get; set; }
        public int? VoucherPercent { get; set; }

        public IEnumerable<OrderDetailResponse> Details { get; set; } = null;
    }
}
