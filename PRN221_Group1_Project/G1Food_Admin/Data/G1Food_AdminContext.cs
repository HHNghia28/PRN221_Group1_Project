using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using G1FOODLibrary.DTO;

namespace G1Food_Admin.Data
{
    public class G1Food_AdminContext : DbContext
    {
        public G1Food_AdminContext (DbContextOptions<G1Food_AdminContext> options)
            : base(options)
        {
        }

        public DbSet<G1FOODLibrary.DTO.ProductResponse> ProductResponse { get; set; } = default!;
    }
}
