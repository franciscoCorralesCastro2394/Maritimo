using Maritimo.Models.Models;
using Maritimo.Web.Models;
using Maritimo.Web.Services;
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
        private readonly BitacoraService _bitacoraService;


        public DashboardController(IHttpClientFactory factory, BitacoraService bitacoraService)
        {
            _http = factory.CreateClient("API");
            _bitacoraService = bitacoraService;
        }

        // GET: DashboardController
        public async Task<ActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");
            var nombreUsuuario = HttpContext.Session.GetString("Usuario");
            var rolUsuario = HttpContext.Session.GetString("Rol");

            await _bitacoraService.RegistrarLog("Ingreso al Dashboard" + nombreUsuuario, "Info");

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }


            ViewBag.Usuario = nombreUsuuario;
            ViewBag.Rol = rolUsuario;

            return View();
        }
     
    }
}
