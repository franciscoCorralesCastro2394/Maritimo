using Maritimo.Data.Context;
using Maritimo.Models.Models;
using Maritimo.Web.Models;
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
    public class RutasController : Controller
    {
        private readonly MaritimoDbContext _context;

        List<EstadoTravesia> estadosTravesia = new List<EstadoTravesia>
        {
            new EstadoTravesia { Id = 1, Estado = "Planeada" },
            new EstadoTravesia { Id = 2, Estado = "En Curso" },
            new EstadoTravesia { Id = 3, Estado = "Completada" },
            new EstadoTravesia { Id = 4, Estado = "Cancelada" }
        };

        public RutasController(MaritimoDbContext context)
        {
            _context = context;
        }

        // GET: Rutas
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

            var maritimoDbContext = _context.Rutas.Include(r => r.Barco).Include(r => r.puertoLlegada).Include(r => r.puertoSalida);
            return View(await maritimoDbContext.ToListAsync());
        }

        // GET: Rutas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Rutas
                .Include(r => r.Barco)
                .Include(r => r.puertoLlegada)
                .Include(r => r.puertoSalida)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruta == null)
            {
                return NotFound();
            }

            return View(ruta);
        }

        // GET: Rutas/Create
        public IActionResult Create()
        {
            ViewData["BarcoId"] = new SelectList(_context.Barcos, "Id", "Matricula");
            ViewData["puertoLlegadaId"] = new SelectList(_context.Puertos, "Id", "Nombre");
            ViewData["puertoSalidaId"] = new SelectList(_context.Puertos, "Id", "Nombre");
            //ViewData["Estado"] = new SelectList(estadosTravesia, "Id", "Estado");



            return View(new Ruta { Estado = "Planeada", FechaPrevistaLlegada = DateTime.Today, FechaPrevistaSalida = DateTime.Today });
        }

        // POST: Rutas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,puertoSalidaId,puertoLlegadaId,BarcoId,FechaPrevistaSalida,FechaPrevistaLlegada,Estado")] Ruta ruta)
        {


            try
            {
                _context.Add(ruta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al crear la ruta: " + ex.Message;
                TempData["Error"] = "Ocurrió un error al crear la ruta";
            }
            ViewData["BarcoId"] = new SelectList(_context.Barcos, "Id", "Matricula", ruta.BarcoId);
            ViewData["puertoLlegadaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoLlegadaId);
            ViewData["puertoSalidaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoSalidaId);
            return View(ruta);
        }

        // GET: Rutas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            ViewData["BarcoId"] = new SelectList(_context.Barcos, "Id", "Matricula", ruta.BarcoId);
            ViewData["puertoLlegadaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoLlegadaId);
            ViewData["puertoSalidaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoSalidaId);
            ViewData["Estado"] = new SelectList(estadosTravesia, "Id", "Estado");
            return View(ruta);
        }

        // POST: Rutas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,puertoSalidaId,puertoLlegadaId,BarcoId,FechaPrevistaSalida,FechaPrevistaLlegada,Estado")] Ruta ruta)
        {

            if (id != ruta.Id)
            {
                return NotFound();
            }

            try
            {
                // Si el estado no ha cambiado, simplemente actualizamos la ruta sin validar la tripulación
                if (estadoRuta(ruta.Id) == ObtenerEstadoTravesia(int.Parse(ruta.Estado)))
                {
                    ruta.Estado = ObtenerEstadoTravesia(int.Parse(ruta.Estado));
                    _context.Entry(ruta).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }


                // Si el estado ha cambiado a "Completada", validamos la tripulación y actualizamos la fecha de cierre y el usuario que cerró la ruta
                if (ObtenerEstadoTravesia(int.Parse(ruta.Estado)) == "Completada" || ObtenerEstadoTravesia(int.Parse(ruta.Estado)) == "Cancelada")
                {
                    ruta.FechaCierre = DateTime.Now;
                    ruta.UsuarioCierre = HttpContext.Session.GetString("Usuario");
                    ruta.Estado = ObtenerEstadoTravesia(int.Parse(ruta.Estado));
                    _context.Entry(ruta).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }

               

                if (ObtenerEstadoTravesia(int.Parse(ruta.Estado)) == "En Curso")
                {

                    // Si el estado ha cambiado, validamos la tripulación antes de actualizar la ruta
                    if (TripulacionValida(ruta.BarcoId))
                    {
                        ruta.Estado = ObtenerEstadoTravesia(int.Parse(ruta.Estado));
                        _context.Entry(ruta).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "No se puede modificar esta ruta por falta de personal";
                    }
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["Error"] = "Error al crear el rol: " + ex.Message;

                if (!RutaExists(ruta.Id))
                {
                    return NotFound();
                }

            }

            ViewData["BarcoId"] = new SelectList(_context.Barcos, "Id", "Matricula", ruta.BarcoId);
            ViewData["puertoLlegadaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoLlegadaId);
            ViewData["puertoSalidaId"] = new SelectList(_context.Puertos, "Id", "Nombre", ruta.puertoSalidaId);
            ViewData["Estado"] = new SelectList(estadosTravesia, "Id", "Estado");

            return View(ruta);
        }

        // GET: Rutas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Rutas
                .Include(r => r.Barco)
                .Include(r => r.puertoLlegada)
                .Include(r => r.puertoSalida)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruta == null)
            {
                return NotFound();
            }

            return View(ruta);
        }

        // POST: Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta != null)
            {
                ruta.Estado = "Archivado";
                //_context.Rutas.Remove(ruta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RutaExists(int id)
        {
            return _context.Rutas.Any(e => e.Id == id);
        }


        // Método para validar la tripulación de un barco antes de permitir modificar la ruta
        private bool TripulacionValida(int barcoId)
        {
            var personal = _context.PersonalBarcosRoles
                .Where(p => p.BarcoId == barcoId)
                .ToList();

            int capitan = personal.Count(p => p.RolId == 8);
            int oficial = personal.Count(p => p.RolId == 9);
            int ingenieros = personal.Count(p => p.RolId == 10);
            int marineros = personal.Count(p => p.RolId == 12);

            return capitan >= 1 &&
                   oficial >= 1 &&
                   ingenieros >= 2 &&
                   marineros >= 5;
        }

        // Método para obtener el estado de la travesía a partir del ID
        private string ObtenerEstadoTravesia(int estadoId)
        {
            var estado = estadosTravesia.FirstOrDefault(e => e.Id == estadoId);
            return estado != null ? estado.Estado : "Desconocido";
        }

        // Método para obtener el estado actual de la ruta desde la base de datos
        private string estadoRuta(int rutaId)
        {
            
            return _context.Rutas
                .AsNoTracking()
                .Where(r => r.Id == rutaId)
                .Select(r => r.Estado)
                .FirstOrDefault() ?? "Desconocido";

        }


        public IActionResult Panel()
        {
            List<Ruta> rutas = _context.Rutas.ToList();

            // Si no hay rutas, asignar null para evitar mostrar secciones vacías en la vista
            if (rutas.Count == 0)
            {
                rutas = null;
                ViewBag.Rutas = rutas;
            }

            return View(generarRuta(rutas));

        }

        private List<RutaVM> generarRuta(List<Ruta> rutas)
        {
            List<RutaVM> rutasVM = new List<RutaVM>();
            foreach (var ruta in rutas)
            {
                rutasVM.Add(new RutaVM
                {
                    puertoSalida = _context.Puertos.Find(ruta.puertoSalidaId)?.Nombre ?? "Desconocido",
                    puertoLlegada = _context.Puertos.Find(ruta.puertoLlegadaId)?.Nombre ?? "Desconocido",
                    FechaPrevistaSalida = ruta.FechaPrevistaSalida,
                    FechaPrevistaLlegada = ruta.FechaPrevistaLlegada,
                    Estado = ruta.Estado,
                    FechaCierre = ruta.FechaCierre,
                    UsuarioCierre = ruta.UsuarioCierre
                });
            }
            return rutasVM;
        }

    }
}
