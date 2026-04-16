using Maritimo.Data.Context;
using Maritimo.Data.Migrations;
using Maritimo.Models.Models;
using Maritimo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maritimo.Web.Controllers
{
    public class BarcoesController : Controller
    {
        private readonly MaritimoDbContext _context;

        public BarcoesController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: Barcoes
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

            List<Puerto> puertos = await _context.Puertos.ToListAsync();
            ViewBag.Puertos = puertos;

            return View(await _context.Barcos.ToListAsync());
        }

        // GET: Barcoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barco = await _context.Barcos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barco == null)
            {
                return NotFound();
            }

            var rolUsuario = HttpContext.Session.GetString("Rol");
            ViewBag.Rol = rolUsuario;

            return View(barco);
        }

        // GET: Barcoes/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.Puertos = _context.Puertos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();

            return View();
        }

        // POST: Barcoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Matricula,Tonelaje,PuertoId,ModeloMotor,PotenciaMotor,HoraUsoMotor")] Barco barco)
        {

            var existeMatricula = await _context.Barcos.AnyAsync(b => b.Matricula == barco.Matricula);
            if (existeMatricula)
            {
                TempData["Error"] = "La matrícula ya existe";
                return View(barco);
            }

            try
            {
                barco.Activo = true;
                _context.Add(barco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors);
                TempData["Error"] = "Error al crear el barco";
            }
            return View(barco);
        }

        // GET: Barcoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barco = await _context.Barcos.FindAsync(id);
            if (barco == null)
            {
                return NotFound();
            }

            ViewBag.Puertos = _context.Puertos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();

            return View(barco);
        }

        // POST: Barcoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Matricula,Tonelaje,PuertoId,ModeloMotor,PotenciaMotor,HoraUsoMotor")] Barco barco)
        {
            if (id != barco.Id)
            {
                return NotFound();
            }

            try
            {
                //_context.Update(barco);
                _context.Entry(barco).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["Error"] = ex.ToString();
                if (!BarcoExists(barco.Id))
                {
                    return NotFound();
                }
            }

            return View(barco);
        }

        // GET: Barcoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barco = await _context.Barcos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barco == null)
            {
                return NotFound();
            }

            return View(barco);
        }

        // POST: Barcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barco = await _context.Barcos.FindAsync(id);
            if (barco != null)
            {
                //_context.Barcos.Remove(barco);
                barco.Activo = false;
                _context.Update(barco);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BarcoExists(int id)
        {
            return _context.Barcos.Any(e => e.Id == id);
        }
    }
}
