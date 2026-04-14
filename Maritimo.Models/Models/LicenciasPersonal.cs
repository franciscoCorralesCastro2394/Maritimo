using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class LicenciasPersonal
    {
        public int Id { get; set; }

        public int PersonalId { get; set; }
        public Personal Personal { get; set; }

        public int LicenciaId { get; set; }
        public LicenciasMaritimas Licencia { get; set; }

    }
}
