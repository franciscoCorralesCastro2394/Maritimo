using Maritimo.Data.Context;
using Maritimo.Models.Models;
using Maritimo.Web.Services;
using Maritimo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maritimo.Web.Controllers
{
    public class RolsController : Controller
    {
        private readonly MaritimoDbContext _context;
        private readonly BitacoraService _bitacoraService;
        public RolsController(MaritimoDbContext context, BitacoraService bitacoraService)
        {
            _context = context;
            _bitacoraService = bitacoraService;
        }

        // GET: Rols
        public async Task<IActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");
            var nombreUsuuario = HttpContext.Session.GetString("Usuario");
            var rolUsuario = HttpContext.Session.GetString("Rol");

            // Verificar si el IdUsuario es nulo
            if (string.IsNullOrEmpty(idUsuario))
            {
                ViewBag.Error = "Usuario no autenticado";
                TempData["Error"] = "Usuario no autenticado";
                return View();
            }


            ViewBag.Rol = rolUsuario;

            ViewBag.UsuarioLoggeado = nombreUsuuario;
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Rols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);

            List<Permiso>? permisos = _context.RolPermisos
                    .Where(rp => rp.RolId == id)
                    .Select(rp => rp.Permiso)
                    .ToList();

            ViewBag.Permisos = permisos;

            var rolUsuario = HttpContext.Session.GetString("Rol");
            ViewBag.Rol = rolUsuario;

            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // GET: Rols/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rols/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Rol rol)
        {
            try
            {
                _context.Add(rol);
                await _context.SaveChangesAsync();
                await _bitacoraService.RegistrarLog($"Creacion del Rol {rol.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrándola o mostrando un mensaje de error
                TempData["Error"] = "Error al crear el rol: " + ex.Message;
            }
            return View(rol);
        }

        // GET: Rols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Rols/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Rol rol)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(rol);
                    await _bitacoraService.RegistrarLog($"Creacion del Rol {rol.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["Error"] = ex.ToString();
                    if (!RolExists(rol.Id))
                        {

                        return NotFound();
                        }
                return View(rol);
            }
        }

        // GET: Rols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // POST: Rols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _bitacoraService.RegistrarLog($"Creacion del Rol {rol.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

        public AsignarPermisoVM crearModelo(int id)
        {
            AsignarPermisoVM vm = new AsignarPermisoVM
            {
                IdRol = id,
                Permisos = _context.Permisos.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                }).ToList(),
                NombreRol = _context.Roles.Where(r => r.Id == id).Select(r => r.Nombre).FirstOrDefault() ?? string.Empty
            };

            return vm;
        }

        public IActionResult Asignar(int id)
        {
            return View(crearModelo(id));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Asignar(AsignarPermisoVM model)
        {
            if (model.PermisosSeleccionados != null && model.IdRol != 0)
            {

                // 
                foreach (var permisoId in model.PermisosSeleccionados)
                {
                    var asignacion = new RolPermiso
                    {
                        RolId = model.IdRol,
                        PermisoId = permisoId
                    };

                    _context.RolPermisos.Add(asignacion);
                    await _bitacoraService.RegistrarLog("Asignación de Permisos " + permisoId.ToString() + " al rol "+  model.IdRol, "Info");

                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Rols");

            }
            else
            {
                TempData["Error"] = "Errores en asignar Permisos";
                return View(crearModelo(model.IdRol));
            }
        }
    }
}
