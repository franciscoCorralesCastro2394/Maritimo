using Maritimo.Models.Models;
using Maritimo.Web.Models;
using Maritimo.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maritimo.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _http;

        public DashboardController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        // GET: DashboardController
        public async Task<ActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");
            var nombreUsuuario = HttpContext.Session.GetString("Usuario");

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _http.PostAsJsonAsync("api/auth/getRol", idUsuario);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Error en obtener el Rol";
                TempData["Error"] = "Error en obtener el Rol";
                return View();
            }

            var rol = await response.Content.ReadFromJsonAsync<RolResponse>();

            if (rol == null)
            {
                ViewBag.Error = "Error al leer la respuesta del servidor";
                TempData["Error"] = "Error al leer la respuesta del servidor";
                return View();
            }

            ViewBag.Usuario = nombreUsuuario;
            ViewBag.Rol = rol.Nombre;

            return View();
        }
     
    }
}
