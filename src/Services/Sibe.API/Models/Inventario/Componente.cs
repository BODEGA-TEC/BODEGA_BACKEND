using Sibe.API.Models.Comprobantes;
using Sibe.API.Models.Historicos;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Componente
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
        public int Cantidad { get; set; }

        [Required]
        public string Estante { get; set; } = string.Empty!;

        [Required]
        public string ActivoBodega { get; set; } = null!;

        public string? ActivoTec { get; set; }

        public string? Observaciones { get; set; }

        // Navegación a la entidad Historico Componente - tiene varios registros propios en la tabla historico
        public ICollection<HistoricoComponente>? HistoricoComponente { get; set; } = new HashSet<HistoricoComponente>(); // garantiza que no haya componentes duplicados en el historial

        // Propiedad de navegación a DetallePrestamoComponente - puede pertenecer a varias boletas pero solo una unica vez a cada una de ellas
        public ICollection<BoletaComponente>? BoletasComponente { get; set; } = new HashSet<BoletaComponente>();
    }
}
