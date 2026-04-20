using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class OrdenesServicio
    {
        public int Id { get; set; }
        public string IdUnico { get; set; }

        public string BarcoId    { get; set; }
        public Barco Barco    { get; set; }

        public string TipoManteniminto { get; set; }

        public string Prioridad { get; set; }

        public string Descipcion{ get; set;}

        public DateTime Fechalimite { get; set; }

        public string Estado { get; set; }

        public string InformeCierre { get; set; }

        public DateTime FechaCreacion { get; set; }
         public DateTime? FechaCierre { get; set; }
         public string UsuarioCreador { get; set; }
         public string UsuarioCierre { get; set; }

    }
}
