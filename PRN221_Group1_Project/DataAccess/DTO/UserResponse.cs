using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string AddressDetail { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }
}
