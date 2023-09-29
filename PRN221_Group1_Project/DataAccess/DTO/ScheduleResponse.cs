using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class ScheduleResponse
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string? Note { get; set; }
    }
}
