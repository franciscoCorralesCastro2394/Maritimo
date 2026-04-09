using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Usuario
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        public int IntentosFallidos { get; set; }

        public DateTime? BloqueadoHasta { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}
