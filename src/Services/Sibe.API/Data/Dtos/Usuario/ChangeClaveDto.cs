namespace Sibe.API.Data.Dtos.Usuario
{
    public class ChangeClaveDto
    {
        public string UsuarioCorreo { get; set; } = string.Empty!;
        public string ClaveNueva { get; set; } = string.Empty!;
        public string ClaveTemporal { get; set; } = string.Empty!;
    }
}
