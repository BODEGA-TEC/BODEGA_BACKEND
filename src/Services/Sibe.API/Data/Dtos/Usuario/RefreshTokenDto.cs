namespace Sibe.API.Data.Dtos.Usuario
{
    public class RefreshTokenDto
    {
        public int UsuarioId { get; set; }
        public string AccessTokenExpirado { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
