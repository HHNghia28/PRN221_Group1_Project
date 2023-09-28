using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderRequest
    {

        public string? Note { get; set; }

        public Guid? UserId { get; set; }

        public string? VoucherCode { get; set; }

        public IEnumerable<OrderDetailRequest> Details { get; set; } = null;
    }
}
