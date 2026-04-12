using Maritimo.Data.Context;
using Maritimo.Models.Models;
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
    public class UsuariosController : Controller
    {
        private readonly MaritimoDbContext _context;

        public UsuariosController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            List<Rol>? roles = _context.UsuarioRoles
                    .Where(ur => ur.UsuarioId == id)
                    .Select(ur => ur.Rol)
                    .ToList();

            if (roles != null) 
            {
                ViewBag.Roles = roles;
            }

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Correo,Password,Activo,FechaCreacion,FechaModificacion,IntentosFallidos,BloqueadoHasta")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,Password,Activo,FechaCreacion,FechaModificacion,IntentosFallidos,BloqueadoHasta")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }


        public AsignarRolesVM crearModelo(int id)
        {
            AsignarRolesVM vm = new AsignarRolesVM
            {
                IdUsuario = id,
                Roles = _context.Roles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList(),
                NombreUsuario = _context.Usuarios.Where(u => u.Id == id).Select(u => u.Nombre).FirstOrDefault() ?? string.Empty
            };

            return vm;
        }

        public IActionResult Asignar(int id)
        {
            return View(crearModelo(id));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Asignar(AsignarRolesVM model)
        {
            if (model.RolesSeleccionados != null && model.IdUsuario != 0)
            {

                // 
                foreach (var rolId in model.RolesSeleccionados)
                {
                    var asignacion = new UsuarioRol
                    {
                        RolId = rolId,
                        UsuarioId = model.IdUsuario 
                    };

                    _context.UsuarioRoles.Add(asignacion);
                }


                _context.SaveChanges();
                return RedirectToAction("Index", "Usuarios");

            }
            else
            {
                TempData["Error"] = "Errores en asignar Roles";
                return View(crearModelo(model.IdUsuario));
            }
        }
    }
}
