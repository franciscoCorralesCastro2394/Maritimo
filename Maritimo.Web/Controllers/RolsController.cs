using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maritimo.Models.Models;
using Maritimo.Data.Context;
using Maritimo.Web.ViewModels;

namespace Maritimo.Web.Controllers
{
    public class RolsController : Controller
    {
        private readonly MaritimoDbContext _context;

        public RolsController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: Rols
        public async Task<IActionResult> Index()
        {
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
            if (ModelState.IsValid)
            {
                _context.Add(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
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
        public ActionResult Asignar(AsignarPermisoVM model)
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
