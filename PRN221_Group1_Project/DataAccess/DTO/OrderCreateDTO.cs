using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderCreateDTO
    {

        public string? Note { get; set; }

        public Guid? UserId { get; set; }

        public Guid? VoucherId { get; set; } = Guid.Empty;

        public Guid? ScheduleId { get; set; } = Guid.Empty;

        public IEnumerable<OrderDetailDTO> Details { get; set; } = null;
    }
}
