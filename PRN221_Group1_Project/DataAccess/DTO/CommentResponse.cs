using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string? Content { get; set; }

        public string? AccountName { get; set; }

        public string? ParentName { get; set; }

        public Guid? ParentCommentId { get; set; }
    }
}
