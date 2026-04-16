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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Maritimo.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MaritimoDbContext _context;
        private readonly BitacoraService _bitacoraService;


        public UsuariosController(MaritimoDbContext context, BitacoraService bitacoraService)
        {
            _context = context;
            _bitacoraService = bitacoraService;
        }

        // GET: Usuarios
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Correo,Password,Activo")] Usuario usuario)
        {
            usuario.Password = ComputeSha256Hash(usuario.Password);

            try
            {
                usuario.FechaCreacion = DateTime.Now;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                await _bitacoraService.RegistrarLog($"Creacion del Usuuario {usuario.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                TempData["Error"] = "Error al crear el usuario: " + ex.Message;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,Password,Activo")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }
            
                try
                {
                    usuario.FechaModificacion = DateTime.Now;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    await _bitacoraService.RegistrarLog($"Edición del Usuario {usuario.Nombre} por {HttpContext.Session.GetString("Usuario")}", "Info");
                    return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["Error"] = "Error al crear el rol: " + ex.Message;

                    if (!UsuarioExists(usuario.Id))
                        {
                            return NotFound();
                        }
                        return View(usuario);
            }
                
            

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
                usuario.Activo = false;
                _context.Entry(usuario).State = EntityState.Modified;
                //_context.Usuarios.Remove(usuario);
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


        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
