using Maritimo.Web.Models;

namespace Maritimo.Web.Services
{
    public class BitacoraService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContext;

        public BitacoraService(IHttpClientFactory factory, IHttpContextAccessor httpContext)
        {
            _http = factory.CreateClient("API");
            _httpContext = httpContext;
        }

        public async Task RegistrarLog(string descripcion, string tipo)
        {

            var bitacora = new BitacoraMV
            {
                DescripcionLog = descripcion,
                TipoLog = tipo,
                FechaLog = DateTime.Now
            };

            var response = await _http.PostAsJsonAsync("api/Bitacora/addBitacora", bitacora);

            // Forma limpia
            response.EnsureSuccessStatusCode();
        }
    }
}
