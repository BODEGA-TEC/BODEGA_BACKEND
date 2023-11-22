using Sibe.API.Models.Comprobantes;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Historicos;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        // Navegación a la entidad Categoria que representa la categoría del equipo.
        [Required]
        public Categoria Categoria { get; set; } = null!;

        // Navegación a la entidad Estado que representa el estado del equipo.
        [Required]
        public Estado Estado { get; set; } = null!;

        [Required]
        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        [Required]
        public string Descripcion { get; set; } = string.Empty!;

        [Required]
        public Condicion Condicion { get; set; }

        [Required]
        public string Estante { get; set; } = string.Empty!;

        public string? Marca { get; set; } = null;

        public string? Modelo { get; set; } = null;

        [Required]
        public string ActivoBodega { get; set; } = null!;

        public string? ActivoTec { get; set; }

        public string? Serie { get; set; }

        public string? Observaciones { get; set; }

        // Navegación a la entidad Historico Equipo - tiene varios registros propios en la tabla historico
        public ICollection<HistoricoEquipo>? HistoricoEquipo { get; set; } = new HashSet<HistoricoEquipo>();    // garantiza que no haya componentes duplicados en el historial

        // Propiedad de navegación a Boleta - puede pertenecer a varias boletas pero solo una unica vez a cada una de ellas
        public ICollection<BoletaEquipo>? BoletasEquipo { get; set; } = new HashSet<BoletaEquipo>();
    }
}
