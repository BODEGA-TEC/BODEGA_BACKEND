using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Categoria? Categoria { get; set; }

        public Estado? Estado { get; set; }

        public string Activo { get; set; } = string.Empty!;

        public string? Serie { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty!;

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string Observaciones { get; set; } = string.Empty;

    }
}
