using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Componente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Categoria Categoria { get; set; } = null!;

        [Required]
        public Estado Estado { get; set; } = null!;

        public string Activo { get; set; } = string.Empty!;

        [Required]
        public string Descripcion { get; set; } = string.Empty!;

        public string Observaciones { get; set; } = string.Empty;
    }
}
