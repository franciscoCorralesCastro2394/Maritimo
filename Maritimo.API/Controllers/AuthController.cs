using Maritimo.API.Models;
using Maritimo.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Maritimo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MaritimoDbContext _context ;

        public AuthController(MaritimoDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u =>
                    u.Correo == request.Correo &&
                    u.Password == ComputeSha256Hash(request.Password));



            if (usuario == null)
            {
                return Unauthorized("Credenciales incorrectas");
            }

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                Roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre)
            });
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
