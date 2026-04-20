using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.ViewModels
{
    public class PanelVM
    {

        public List<RutaVM> Rutas { get; set; }

        //Filtros  
        public string filtroBarco { get; set; }
        public string filtroPuerto { get; set; }

        public int? filtroBarcoId { get; set; }
        public int? filtroPuertoId { get; set; }

        public List<SelectListItem> Barcos { get; set; }
        public List<SelectListItem> Puertos { get; set; }



    }
}
