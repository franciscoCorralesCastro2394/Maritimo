using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.ViewModels
{
    public class AsignarLicVM
    {
        public int IdPersonal { get; set; }

        public string NombrePersonal { get; set; }

        public IEnumerable<SelectListItem> Licencias { get; set; }

        public List<int> LicenciasSeleccionados { get; set; }
    }
}
