using Maritimo.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        //Filtros  
        public string filtroBarco { get; set; }
        public string filtroPuerto { get; set; }

        public int? filtroBarcoId { get; set; }
        public int? filtroPuertoId { get; set; }

        public List<SelectListItem> Barcos { get; set; }
        public List<SelectListItem> Puertos { get; set; }
    }
}
