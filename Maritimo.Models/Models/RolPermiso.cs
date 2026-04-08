using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class RolPermiso
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public int PermisoId { get; set; }
        public Permiso Permiso { get; set; }
    }
}
