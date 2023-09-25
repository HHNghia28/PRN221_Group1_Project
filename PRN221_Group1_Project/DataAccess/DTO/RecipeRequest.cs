using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class RecipeRequest
    {

        public double Quantity { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? WarehouseItemId { get; set; }
    }
}
