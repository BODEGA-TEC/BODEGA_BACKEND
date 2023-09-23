using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
