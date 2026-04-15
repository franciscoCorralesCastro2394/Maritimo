using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maritimo.Data.Context;
using Maritimo.Models.Models;

namespace Maritimo.Web.Controllers
{
    public class PuertoesController : Controller
    {
        private readonly MaritimoDbContext _context;

        public PuertoesController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: Puertoes
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
            return View(await _context.Puertos.ToListAsync());
        }

        // GET: Puertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puerto = await _context.Puertos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (puerto == null)
            {
                return NotFound();
            }

            var rolUsuario = HttpContext.Session.GetString("Rol");
            ViewBag.Rol = rolUsuario;

            return View(puerto);
        }

        // GET: Puertoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Puertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Puerto puerto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puerto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(puerto);
        }

        // GET: Puertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puerto = await _context.Puertos.FindAsync(id);
            if (puerto == null)
            {
                return NotFound();
            }
            return View(puerto);
        }

        // POST: Puertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Puerto puerto)
        {
            if (id != puerto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puerto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuertoExists(puerto.Id))
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
            return View(puerto);
        }

        // GET: Puertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puerto = await _context.Puertos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (puerto == null)
            {
                return NotFound();
            }

            return View(puerto);
        }

        // POST: Puertoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puerto = await _context.Puertos.FindAsync(id);
            if (puerto != null)
            {
                _context.Puertos.Remove(puerto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuertoExists(int id)
        {
            return _context.Puertos.Any(e => e.Id == id);
        }
    }
}
