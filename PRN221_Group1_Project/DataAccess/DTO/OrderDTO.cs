using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string? Note { get; set; }

        public Guid? StatusId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? ScheduleId { get; set; }

        public Guid? VoucherId { get; set; }

        public IEnumerable<OrderDetailDTO> Details { get; set; } = null;
    }
}
