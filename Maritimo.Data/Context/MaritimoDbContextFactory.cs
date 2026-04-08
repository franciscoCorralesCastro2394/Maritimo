using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Maritimo.Data.Context
{
    // Design-time factory for EF Core tools (migrations, scaffolding, etc.).
    // This ensures the tools can create a MaritimoDbContext instance even when
    // the application's DI container isn't available at design time.
    public class MaritimoDbContextFactory : IDesignTimeDbContextFactory<MaritimoDbContext>
    {
        public MaritimoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MaritimoDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=MaritimoDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new MaritimoDbContext(optionsBuilder.Options);
        }
    }
}
