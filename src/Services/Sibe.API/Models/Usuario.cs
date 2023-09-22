using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Usuario
    {
        [Key]
        [MaxLength(10)] // El carné tiene una longitud máxima de 10 caracteres.
        public string Carne { get; set; } = null!;

        public Rol Rol { get; set; } = null!;

        [Required]
        public string Nombre { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required]
        [EmailAddress]
        public string CorreoTec { get; set; } = null!;
    }
}
