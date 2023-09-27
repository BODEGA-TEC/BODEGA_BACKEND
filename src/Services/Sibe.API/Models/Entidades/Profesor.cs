using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Entidades
{
    public class Profesor
    {
        [Key]
        public int Id { get; set; }

        // Navegación a la entidad Departamento que representa el area academica a la que pertenece el profesor
        [Required]
        public Departamento Departamento { get; set; } = null!;

        [Required]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value.ToUpper(); } // Convierte a mayúsculas antes de asignar
        }

        [Required]
        public string PrimerApellido { get; set; } = null!;

        [Required]
        public string SegundoApellido { get; set; } = null!;

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@itcr\.ac\.cr$", ErrorMessage = "El correo debe tener el dominio @itcr.ac.cr")]
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