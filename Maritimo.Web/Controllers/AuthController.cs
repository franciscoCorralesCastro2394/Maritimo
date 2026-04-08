using Maritimo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maritimo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;

        public AuthController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            //
            var response = await _http.PostAsJsonAsync("api/auth/login", userLogin);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Credenciales incorrectas";
                TempData["Error"] = "Credenciales incorrectas";
                return View();
            }

            var usuario = await response.Content.ReadFromJsonAsync<UsuarioResponseViewModel>();

            if (usuario == null)
            {
                ViewBag.Error = "Error al leer la respuesta del servidor";
                TempData["Error"] = "Error al leer la respuesta del servidor";
                return View();
            }

            // Safely obtain fields from the dynamic object
            string? idUsuario = usuario.Id.ToString();
            string? nombre = (string?)usuario.Nombre;
            string? correo = (string?)usuario.Correo;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo))
            {
                ViewBag.Error = "Datos de usuario incompletos";
                TempData["Error"] = "Datos de usuario incompletos";
                return View();
            }

            // Ensure session is available before using it
            var session = HttpContext.Session;
            if (session == null)
            {
                ViewBag.Error = "Sesión no disponible";
                TempData["Error"] = "Sesión no disponible";
                return View();
            }

            if (!session.IsAvailable)
            {
                await session.LoadAsync();
                if (!session.IsAvailable)
                {
                    ViewBag.Error = "No se pudo inicializar la sesión";
                    TempData["Error"] = "No se pudo inicializar la sesión";
                    return View();
                }
            }

            session.SetString("Usuario", nombre);
            session.SetString("Correo", correo);
            session.SetString("IdUsuario", idUsuario);

            return RedirectToAction("Index", "Home");
        }
    }
}
