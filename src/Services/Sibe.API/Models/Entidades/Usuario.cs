using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Entidades
{
    public class Usuario
    {
        [Key]
        [MaxLength(10)] // El carné tiene una longitud máxima de 10 caracteres.
        public string Carne { get; set; } = null!;

        // Navegación a la entidad Rol que representa el rol del usuario.
        [Required]
        public Rol Rol { get; set; } = null!;

        [Required]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value.ToUpper(); } // Convierte a mayúsculas antes de asignar
        }

        public byte[] ClaveHash { get; set; } = Array.Empty<byte>();

        public byte[] ClaveSalt { get; set; } = Array.Empty<byte>();

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@estudiantec\.cr$", ErrorMessage = "El correo debe tener el dominio @estudiantec.cr")]
        public string Correo
        {
            get { return _correo; }
            set { _correo = value.ToLower(); } // Convierte a minúsculas antes de asignar
        }

        [NotMapped]
        private string _nombre = null!;
        [NotMapped]
        private string _correo = null!;
    }
}
