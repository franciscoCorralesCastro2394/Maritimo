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
    public class LicenciasMaritimasController : Controller
    {
        private readonly MaritimoDbContext _context;

        public LicenciasMaritimasController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: LicenciasMaritimas
        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario tiene un rol en la sesión
            var rolUsuario = HttpContext.Session.GetString("Rol");

            if (rolUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Rol = rolUsuario;

            return View(await _context.LicenciasMaritimas.ToListAsync());
        }

        // GET: LicenciasMaritimas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolUsuario = HttpContext.Session.GetString("Rol");

            if (rolUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Rol = rolUsuario;

            var licenciasMaritimas = await _context.LicenciasMaritimas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenciasMaritimas == null)
            {
                return NotFound();
            }

            return View(licenciasMaritimas);
        }

        // GET: LicenciasMaritimas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LicenciasMaritimas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreLicencia,FechaExpiracion")] LicenciasMaritimas licenciasMaritimas)
        {
            try
            {
                _context.Add(licenciasMaritimas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors);
                TempData["Error"] = error;
            }
            return View(licenciasMaritimas);
        }

        // GET: LicenciasMaritimas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenciasMaritimas = await _context.LicenciasMaritimas.FindAsync(id);
            if (licenciasMaritimas == null)
            {
                return NotFound();
            }
            return View(licenciasMaritimas);
        }

        // POST: LicenciasMaritimas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreLicencia,FechaExpiracion")] LicenciasMaritimas licenciasMaritimas)
        {
            if (id != licenciasMaritimas.Id)
            {
                return NotFound();
            }
                try
                {
                    _context.Update(licenciasMaritimas);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!LicenciasMaritimasExists(licenciasMaritimas.Id))
                    {
                        TempData["Error"] = ex.ToString();
                        return NotFound();
                    }
                }
               
            
            return View(licenciasMaritimas);
        }

        // GET: LicenciasMaritimas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenciasMaritimas = await _context.LicenciasMaritimas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenciasMaritimas == null)
            {
                return NotFound();
            }

            return View(licenciasMaritimas);
        }

        // POST: LicenciasMaritimas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var licenciasMaritimas = await _context.LicenciasMaritimas.FindAsync(id);
            if (licenciasMaritimas != null)
            {
                _context.LicenciasMaritimas.Remove(licenciasMaritimas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenciasMaritimasExists(int id)
        {
            return _context.LicenciasMaritimas.Any(e => e.Id == id);
        }
    }
}
