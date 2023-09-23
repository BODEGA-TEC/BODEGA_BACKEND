using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Profesor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Departamento Departamento { get; set; } = null!;

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string PrimerApellido { get; set; } = null!;

        [Required]
        public string SegundoApellido { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;
    }
}