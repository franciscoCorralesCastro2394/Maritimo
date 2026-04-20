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
    public class OrdenesServiciosController : Controller
    {
        private readonly MaritimoDbContext _context;

        public OrdenesServiciosController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: OrdenesServicios
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrdenesServicio.ToListAsync());
        }

        // GET: OrdenesServicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenesServicio = await _context.OrdenesServicio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenesServicio == null)
            {
                return NotFound();
            }

            return View(ordenesServicio);
        }

        // GET: OrdenesServicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrdenesServicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUnico,BarcoId,TipoManteniminto,Prioridad,Descipcion,Fechalimite,Estado,InformeCierre,FechaCreacion,FechaCierre,UsuarioCreador,UsuarioCierre")] OrdenesServicio ordenesServicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordenesServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordenesServicio);
        }

        // GET: OrdenesServicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenesServicio = await _context.OrdenesServicio.FindAsync(id);
            if (ordenesServicio == null)
            {
                return NotFound();
            }
            return View(ordenesServicio);
        }

        // POST: OrdenesServicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUnico,BarcoId,TipoManteniminto,Prioridad,Descipcion,Fechalimite,Estado,InformeCierre,FechaCreacion,FechaCierre,UsuarioCreador,UsuarioCierre")] OrdenesServicio ordenesServicio)
        {
            if (id != ordenesServicio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenesServicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenesServicioExists(ordenesServicio.Id))
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
            return View(ordenesServicio);
        }

        // GET: OrdenesServicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenesServicio = await _context.OrdenesServicio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenesServicio == null)
            {
                return NotFound();
            }

            return View(ordenesServicio);
        }

        // POST: OrdenesServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenesServicio = await _context.OrdenesServicio.FindAsync(id);
            if (ordenesServicio != null)
            {
                _context.OrdenesServicio.Remove(ordenesServicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenesServicioExists(int id)
        {
            return _context.OrdenesServicio.Any(e => e.Id == id);
        }
    }
}
