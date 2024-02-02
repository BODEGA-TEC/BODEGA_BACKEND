namespace Sibe.API.Data.Dtos.Usuario
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty!;
        public int Rol { get; set; }
        public string AccessToken { get; set; } = string.Empty!;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
