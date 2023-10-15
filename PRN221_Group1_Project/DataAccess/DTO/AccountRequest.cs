using G1FOODLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class AccountRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string AddressDetail { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public Guid? RoleId { get; set; }

        public Guid? StatusId { get; set; }
    }
}
