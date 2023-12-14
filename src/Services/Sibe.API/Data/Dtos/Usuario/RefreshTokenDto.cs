namespace Sibe.API.Data.Dtos.Usuario
{
    public class RefreshTokenDto
    {
        public string TokenExpirado { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
