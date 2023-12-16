using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Usuario
{
    public class RegisterUsuarioDto
    {
        public string Nombre { get; set; } = string.Empty!;
        public string Username { get; set; } = string.Empty!;
        public string Correo { get; set; } = string.Empty!;
        public string Clave { get; set; } = string.Empty!;
        public Rol Rol { get; set; } = Rol.ASISTENTE;
    }
}
