using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Permiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<RolPermiso> RolPermisos { get; set; }
    }
}
