namespace Maritimo.Web.ViewModels
{
    public class UsuarioResponseViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public List<string> Roles { get; set; }
    }
}
