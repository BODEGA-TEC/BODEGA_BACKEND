using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        // Navegación a la entidad Categoria que representa la categoría del componente.
        [Required]
        public Categoria Categoria { get; set; } = null!;

        // Navegación a la entidad Estado que representa el estado del componente.
        [Required]
        public Estado Estado { get; set; } = null!;

        [Required]
        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        [Required]
        public string Descripcion { get; set; } = string.Empty!;

        [Required]
        public string ActivoBodega { get; set; } = null!;

        public string? Marca { get; set; } = null;

        public string? Modelo { get; set; } = null;

        public string? ActivoTec { get; set; }

        public string? Serie { get; set; }

        public string? Observaciones { get; set; }
    }
}
