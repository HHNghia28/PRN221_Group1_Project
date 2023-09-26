using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class ProductResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int? SalePercent { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public string? Category { get; set; }

        public Guid? StatusId { get; set; }

        public Guid? CategogyId { get; set; }

        public IEnumerable<CommentResponse> Comments { get; set; }
    }
}
