using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Personal
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string IdentificacionUnica { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public DateTime FechaContratacion { get; set; }
    }
}
