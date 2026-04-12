using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.ViewModels
{
    public class AsignarRolesVM
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public List<int> RolesSeleccionados { get; set; }

    }
}
