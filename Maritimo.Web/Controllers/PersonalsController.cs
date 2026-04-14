using Maritimo.Data.Context;
using Maritimo.Models.Models;
using Maritimo.Web.Models;
using Maritimo.Web.Services;
using Maritimo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maritimo.Web.Controllers
{
    public class PersonalsController : Controller
    {
        private readonly MaritimoDbContext _context;

        private readonly HttpClient _http;

        private readonly BitacoraService _bitacoraService;


        public PersonalsController(IHttpClientFactory factory, MaritimoDbContext context, BitacoraService bitacoraService)
        {
            _http = factory.CreateClient("API");
            _context = context;
            _bitacoraService = bitacoraService;
        }


        // GET: Personals
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


            var maritimoDbContext = _context.Personales.Include(p => p.Rol);
            return View(await maritimoDbContext.ToListAsync());
        }

        // GET: Personals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales
                .Include(p => p.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personal == null)
            {
                return NotFound();
            }

            var rolUsuario = HttpContext.Session.GetString("Rol");
            ViewBag.Rol = rolUsuario;
            return View(personal);
        }

        // GET: Personals/Create
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: Personals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto,IdentificacionUnica,RolId,FechaContratacion")] Personal personal)
        {

            personal.Rol = await _context.Roles.FindAsync(personal.RolId);

            try
            {
                _context.Add(personal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors);
                TempData["Error"] = error;

            } 
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", personal.RolId);
            return View(personal);
        }

        // GET: Personals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales.FindAsync(id);
            if (personal == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", personal.RolId);
            return View(personal);
        }

        // POST: Personals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,IdentificacionUnica,RolId,FechaContratacion")] Personal personal)
        {
            if (id != personal.Id)
            {
                return NotFound();
            }
           
                try
                {
                    _context.Update(personal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["Error"] = ex.ToString();
                    if (!PersonalExists(personal.Id))
                    {
                        return NotFound();
                    }
                   

                ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", personal.RolId);
                return View(personal);
            }
                
            

        }

        // GET: Personals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales
                .Include(p => p.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        // POST: Personals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personal = await _context.Personales.FindAsync(id);
            if (personal != null)
            {
                _context.Personales.Remove(personal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalExists(int id)
        {
            return _context.Personales.Any(e => e.Id == id);
        }

        public AsignarLicVM crearModelo(int id)
        {
            AsignarLicVM vm = new AsignarLicVM
            {
                IdPersonal = id,
                Licencias  = _context.LicenciasMaritimas.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.NombreLicencia
                }).ToList(),
                NombrePersonal = _context.Personales.Where(p => p.Id == id).Select(p => p.NombreCompleto).FirstOrDefault() ?? string.Empty
            };

            return vm;
        }

        public IActionResult AsignarLic(int id)
        {
            return View(crearModelo(id));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Asignar(AsignarLicVM model)
        {
            if (model.LicenciasSeleccionados != null && model.IdPersonal != 0)
            {

                // 
                foreach (var licenciaId in model.LicenciasSeleccionados)
                {
                    var asignacion = new LicenciasPersonal
                    {
                        LicenciaId = licenciaId,
                        PersonalId = model.IdPersonal
                    };

                    _context.LicenciasPersonal.Add(asignacion);
                    using var bitacora = _bitacoraService.RegistrarLog("Asignación de Licencias " + licenciaId.ToString() + " al personal " + model.IdPersonal, "Info");

                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Personals");

            }
            else
            {
                TempData["Error"] = "Errores en asignar Licencias";
                return View(crearModelo(model.IdPersonal));
            }
        }
    }
}
