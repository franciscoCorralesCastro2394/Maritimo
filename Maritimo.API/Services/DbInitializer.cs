using Maritimo.Data.Context;
using Maritimo.Models.Models;
using System.Security.Cryptography;
using System.Text;

namespace Maritimo.API.Services
{
    public class DbInitializer
    {

        private readonly MaritimoDbContext _context;

        public DbInitializer(MaritimoDbContext context)
        {
            _context = context;
        }


        public void Seed() 
        {
            if (!_context.Roles.Any()) 
            {
                List<Rol> roles = new List<Rol>
                {
                    new Rol { Nombre = "Administrador" },
                    new Rol { Nombre = "Capitan" },
                    new Rol { Nombre = "Primer Oficial" },
                    new Rol { Nombre = "Ingeniero" },
                    new Rol { Nombre = "Personal Base" },
                    new Rol { Nombre = "Marineros" }
                };

                _context.Roles.AddRange(roles);
                _context.SaveChanges();

            }

            if (!_context.Permisos.Any()) 
            {
                List<Permiso> permisos = new List<Permiso>
                {
                    new Permiso { Nombre = "Create" },
                    new Permiso { Nombre = "Edit" },
                    new Permiso { Nombre = "Delete" },
                    new Permiso { Nombre = "Read" },
                    new Permiso { Nombre = "View" }

                };

                _context.Permisos.AddRange(permisos);
                _context.SaveChanges();
            }

            if (!_context.RolPermisos.Any())
            {
                List<RolPermiso> rolPermisos = new List<RolPermiso>
                {
                    new RolPermiso {  RolId = 10, PermisoId = 1 },
                    new RolPermiso {  RolId = 10, PermisoId = 5 }

                };

                _context.RolPermisos.AddRange(rolPermisos);
                _context.SaveChanges();
            }

            if (!_context.Usuarios.Any())
            {
                List<Usuario> usuarios = new List<Usuario>
                {
                    new Usuario {  Nombre = "Admin", Password = ComputeSha256Hash("1234"), Correo = "admin@test.com", Activo = true, FechaCreacion = DateTime.Now }

                };

                _context.Usuarios.AddRange(usuarios);
                _context.SaveChanges();
            }


            if (!_context.UsuarioRoles.Any()) 
            {
                
                List<UsuarioRol> usuarioRoles = new List<UsuarioRol>
                {
                    new UsuarioRol {  UsuarioId = 2, RolId = 10 }
                };

                _context.UsuarioRoles.AddRange(usuarioRoles);
                _context.SaveChanges();

            }


        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}
