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
        public DbSet<Puerto> Puertos { get; set; }
        public DbSet<Barco> Barcos { get; set; }

        public DbSet<PersonalBarcoRol> PersonalBarcosRoles { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<OrdenesServicio> OrdenesServicio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<PersonalBarco>()
            //    .HasOne(pb => pb.Rol)
            //    .WithMany()
            //    .HasForeignKey(pb => pb.RolId)
            //    .OnDelete(DeleteBehavior.Restrict); // 🔥 CAMBIO CLAVE


        }
    }
}
