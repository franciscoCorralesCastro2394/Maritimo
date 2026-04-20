using Maritimo.Models.Models;

namespace Maritimo.Web.ViewModels
{
    public class RutaVM
    {
        public string puertoSalida { get; set; }

        public string puertoLlegada { get; set; }

        public DateTime FechaPrevistaSalida { get; set; }
        public DateTime FechaPrevistaLlegada { get; set; }

        public string Estado { get; set; }

        public DateTime? FechaCierre { get; set; }

        public string? UsuarioCierre { get; set; }
    }
}
