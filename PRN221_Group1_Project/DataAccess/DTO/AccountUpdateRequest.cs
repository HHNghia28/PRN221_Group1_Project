using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class AccountUpdateRequest
    {
        public string Name { get; set; } = null!;

        public string AddressDetail { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public Guid? RoleId { get; set; }

        public Guid? StatusId { get; set; }
    }
}
