using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Usuario
{
    public class RegisterUsuarioDto
    {
        public string Carne { get; set; } = string.Empty!;

        public string Nombre { get; set; } = string.Empty!;

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!_])[A-Za-z\d@#$%^&+=!_]{8,16}$", 
            ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres y contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")] 
        public string Clave { get; set; } = string.Empty!;

        public int RolId { get; set; } = 3; // Asistente por defecto

        public string Correo { get; set; } = string.Empty!;
    }
}
