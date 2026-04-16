using Maritimo.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maritimo.Web.Models
{
    public class BarcosVM
    {
        public Barco barco { get; set; }

        public IEnumerable<SelectListItem> Puertos { get; set; }
    }
}
