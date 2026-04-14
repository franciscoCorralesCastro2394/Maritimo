using Maritimo.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Data.Context
{
    public class MaritimoDbContext : DbContext
    {
        public MaritimoDbContext(DbContextOptions<MaritimoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }
        public DbSet<Personal> Personales { get; set; }
        public DbSet<LicenciasMaritimas> LicenciasMaritimas { get; set; }
        public DbSet<LicenciasPersonal> LicenciasPersonal { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // 🌱 Seed (opcional pero recomendado)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Capitan" },
                new Rol { Id = 3, Nombre = "Primer Oficial" },
                new Rol { Id = 4, Nombre = "Ingeniero" },
                new Rol { Id = 5, Nombre = "Personal Base" },
                new Rol { Id = 6, Nombre = "Marineros" }
            );
        }
    }
}
