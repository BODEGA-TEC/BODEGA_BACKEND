using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Usuario
{
    public class RegisterUsuarioDto
    {
        public string Carne { get; set; } = string.Empty!;
        public string Nombre { get; set; } = string.Empty!;
        public string Clave { get; set; } = string.Empty!;
        public int RolId { get; set; } = 3; // Asistente por defecto
        public string Correo { get; set; } = string.Empty!;
    }
}
