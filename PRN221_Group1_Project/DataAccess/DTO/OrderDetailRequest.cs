using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderDetailRequest
    {

        public double Quantity { get; set; }

        public string? Note { get; set; }

        public Guid? ProductId { get; set; } = Guid.Empty;
    }
}
