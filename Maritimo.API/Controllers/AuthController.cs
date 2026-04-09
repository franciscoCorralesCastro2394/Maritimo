using Maritimo.API.Models;
using Maritimo.Data.Context;
using Maritimo.Data.Migrations;
using Maritimo.Models.Models;
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
            // Obtener configuración de seguridad (ejemplo, no se usa en este código)
            var config = SecurityConfig.SecurityConfig.GetSettings();

            string hashedPassword = ComputeSha256Hash(request.Password);

            Usuario? usuarioAuth = await _context.Usuarios.FirstOrDefaultAsync(u =>
                    u.Correo == request.Correo &&
                    u.Password == hashedPassword && u.Activo);

            Usuario? usuarioExist = await _context.Usuarios.FirstOrDefaultAsync(u =>
                                            u.Correo == request.Correo && u.Activo);

            //
            if (usuarioAuth == null && usuarioExist == null)
            {
                return Unauthorized("Credenciales incorrectas");
            }


            // Verificar si el usuario existe y si está bloqueado
            if (usuarioExist != null && usuarioAuth == null)
            {
                if (usuarioExist.BloqueadoHasta != null && usuarioExist.BloqueadoHasta > DateTime.Now)
                {
                    return Unauthorized($"Usuario bloqueado hasta {usuarioExist.BloqueadoHasta.Value}");
                }
                else 
                {
                    usuarioExist.IntentosFallidos += 1;

                    if (usuarioExist.IntentosFallidos >= config.MaxFailedAttempts)
                    {
                        usuarioExist.BloqueadoHasta = DateTime.Now.AddMinutes(config.LockMinutes);
                        usuarioExist.IntentosFallidos = 0; // Reiniciar intentos fallidos después de bloquear
                    }
                    await _context.SaveChangesAsync();
                    return Unauthorized($"Usuario bloqueado temporalmente debido a múltiples intentos fallidos.");

                }
            }
            
            usuarioAuth.IntentosFallidos = 0;
            await _context.SaveChangesAsync();

            return Ok(new
                    {
                        usuarioAuth.Id,
                        usuarioAuth.Nombre,
                        usuarioAuth.Correo
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
