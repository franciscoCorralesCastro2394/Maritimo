using Maritimo.Data.Context;
using Maritimo.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maritimo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : Controller
    {
        private readonly MaritimoDbContext _context;

        public BitacoraController(MaritimoDbContext context)
        {
            _context = context;
        }


        [HttpPost("addBitacora")]
        public async Task<IActionResult> addBitacora([FromBody] Bitacora bitacora)
        {
            if (bitacora == null) 
            {
                return BadRequest("El objeto bitacora no puede ser nulo.");


            }

                _context.Bitacoras.Add(bitacora);
                await _context.SaveChangesAsync();



            return Ok(new
            {
                bitacora.Id
            });

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
