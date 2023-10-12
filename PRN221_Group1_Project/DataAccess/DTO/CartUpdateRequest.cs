using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class CartUpdateRequest
    {
        public Guid Id { get; set; }

        public double Quantity { get; set; }
    }
}
