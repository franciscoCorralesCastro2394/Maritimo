using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class PersonalBarcoRol
    {
        public int Id { get; set; }

        public int PersonalId { get; set; }
        public Personal Personal { get; set; }

        public int BarcoId { get; set; }
        public Barco Barco { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
