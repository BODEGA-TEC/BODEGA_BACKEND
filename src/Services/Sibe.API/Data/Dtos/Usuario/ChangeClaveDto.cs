namespace Sibe.API.Data.Dtos.Usuario
{
    public class ChangeClaveDto
    {
        public int UsuarioId { get; set; }
        public string ClaveNueva { get; set; } = string.Empty!;
        public string ClaveTemporal { get; set; } = string.Empty!;
    }
}
