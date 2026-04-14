using Maritimo.Web.Models;
using Maritimo.Web.Services;
using Maritimo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maritimo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;
        private readonly BitacoraService _bitacoraService;

        public AuthController(IHttpClientFactory factory, BitacoraService bitacoraService)
        {
            _http = factory.CreateClient("API");
            _bitacoraService = bitacoraService;

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
                // Log the authentication error with details

                await _bitacoraService.RegistrarLog("Error de autenticación", "Info");
                
                return View();
            }

            var usuario = await response.Content.ReadFromJsonAsync<UsuarioResponseViewModel>();

            if (usuario == null)
            {
                ViewBag.Error = "Error al leer la respuesta del servidor";
                TempData["Error"] = "Error al leer la respuesta del servidor";

                await _bitacoraService.RegistrarLog("Error al leer la respuesta del servidor", "Info");

                return View();
            }



            // Realizar la solicitud HTTP para obtener el rol del usuario
            var responseRol = await _http.PostAsJsonAsync("api/auth/getRol", usuario.Id.ToString());

            // Verificar si la respuesta fue exitosa
            if (!responseRol.IsSuccessStatusCode)
            {
                ViewBag.Error = "Error en obtener el Rol";
                TempData["Error"] = "Error en obtener el Rol";

                await _bitacoraService.RegistrarLog("Error en obtener el Rol", "Info");
                return View();
            }

            // Leer la respuesta y obtener el rol
            var rol = await responseRol.Content.ReadFromJsonAsync<RolResponse>();

            // Safely obtain fields from the dynamic object
            string? idUsuario = usuario.Id.ToString();
            string? nombre = (string?)usuario.Nombre;
            string? correo = (string?)usuario.Correo;
            string? rolUsuaurio = (string?)rol.Nombre;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo))
            {
                ViewBag.Error = "Datos de usuario incompletos";
                TempData["Error"] = "Datos de usuario incompletos";

                await _bitacoraService.RegistrarLog("Datos de usuario incompletos", "Info");

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
            session.SetString("Rol", rolUsuaurio);

            await _bitacoraService.RegistrarLog("Usuario loggeado con exito: " + nombre, "Info");

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
