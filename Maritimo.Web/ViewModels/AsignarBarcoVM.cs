using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.ViewModels
{
    public class AsignarBarcoVM
    {
        public int IdPersonal { get; set; }
        public int IdRol { get; set; }

        public string NombrePersonal { get; set; }

        public IEnumerable<SelectListItem> Barcos { get; set; }

        public List<int> BarcosSeleccionados { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public List<int> RolesSeleccionados { get; set; }

    }
}
