using System;
using System.Collections.Generic;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Ruta
    {

        public int Id { get; set; }

        public int puertoSalidaId { get; set; }

        public Puerto puertoSalida { get; set; }

        public int puertoLlegadaId { get; set; }

        public Puerto puertoLlegada { get; set; }

        public int BarcoId { get; set; }

        public Barco Barco { get; set; }


        public DateTime FechaPrevistaSalida { get; set; }
        public DateTime FechaPrevistaLlegada { get; set; }


        public string Estado { get; set; } = "Planeada";
    }
}
