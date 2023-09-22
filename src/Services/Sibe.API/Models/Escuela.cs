using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Escuela
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
