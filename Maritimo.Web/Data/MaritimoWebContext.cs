using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Maritimo.Models.Models;

namespace Maritimo.Web.Data
{
    public class MaritimoWebContext : DbContext
    {
        public MaritimoWebContext (DbContextOptions<MaritimoWebContext> options)
            : base(options)
        {
        }

        public DbSet<Maritimo.Models.Models.Rol> Rol { get; set; } = default!;
    }
}
