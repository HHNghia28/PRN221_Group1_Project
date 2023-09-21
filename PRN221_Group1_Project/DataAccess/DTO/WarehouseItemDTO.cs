using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class WarehouseItemDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? Unitname { get; set; }

        public Guid? CategogyItemId { get; set; }

        public Guid? UnitId { get; set; }
    }
}
