using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.ViewModels
{
    public class AsignarPermisoVM
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } 

        public IEnumerable<SelectListItem> Permisos { get; set; }

        public List<int> PermisosSeleccionados { get; set; }
    }
}
