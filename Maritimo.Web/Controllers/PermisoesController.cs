using Azure;
using Maritimo.Data.Context;
using Maritimo.Models.Models;
using Maritimo.Web.Models;
using Maritimo.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Maritimo.Web.Controllers
{
    public class PermisoesController : Controller
    {
        private readonly MaritimoDbContext _context;

        private readonly HttpClient _http;

        private readonly BitacoraService _bitacoraService;


        public PermisoesController(IHttpClientFactory factory,MaritimoDbContext context, BitacoraService bitacoraService)
        {
            _http = factory.CreateClient("API");
            _context = context;
            _bitacoraService = bitacoraService;
        }

        // GET: Permisoes
        public async Task<IActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");
            var nombreUsuuario = HttpContext.Session.GetString("Usuario");

            // Verificar si el IdUsuario es nulo
            if (string.IsNullOrEmpty(idUsuario))
            {
                ViewBag.Error = "Usuario no autenticado";
                TempData["Error"] = "Usuario no autenticado";
                return View();
            }

            // Realizar la solicitud HTTP para obtener el rol del usuario
            var response = await _http.PostAsJsonAsync("api/auth/getRol", idUsuario);

            // Verificar si la respuesta fue exitosa
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Error en obtener el Rol";
                TempData["Error"] = "Error en obtener el Rol";
                return View();
            }

            // Leer la respuesta y obtener el rol
            var rol = await response.Content.ReadFromJsonAsync<RolResponse>();

            ViewBag.Rol = rol.Nombre;

            ViewBag.UsuarioLoggeado = nombreUsuuario;

            return View(await _context.Permisos.ToListAsync());
        }

        // GET: Permisoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Permisoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Permiso permiso)
        {
            try
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                await _bitacoraService.RegistrarLog($"Creacion del Permiso {permiso.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                ViewBag.Error = "Error al crear el permiso: " + ex.Message;
                TempData["Error"] = "Error al crear el permiso: " + ex.Message;
            }
            
            return View(permiso);
        }

        // GET: Permisoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // POST: Permisoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Permiso permiso)
        {
            if (id != permiso.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                    await _bitacoraService.RegistrarLog($"Edición del Permiso {permiso.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                    return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException ex)
                {
                TempData["Error"] = ex.ToString();
                if (!PermisoExists(permiso.Id))
                    {

                        return NotFound();
                    }
            return View(permiso);

            }

        }

        // GET: Permisoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }
    }
}
