using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Bitacora
    {
        public int Id { get; set; }
        public string DescripcionLog { get; set; }
        public string TipoLog { get; set; }

        public DateTime? FechaLog { get; set; }

    }
}
