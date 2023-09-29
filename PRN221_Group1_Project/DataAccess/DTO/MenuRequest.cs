using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class MenuRequest
    {
        public double Quantity { get; set; }

        public Guid? ScheduleId { get; set; }

        public Guid? ProductId { get; set; }
    }
}
