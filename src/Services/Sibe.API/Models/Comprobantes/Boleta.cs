using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Comprobantes
{
    public class Boleta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = TimeZoneHelper.Now();

        [Required]
        public TipoComprobantePrestamo Tipo { get; set; }

        [Required]
        public string Descripcion { get; set; } = null!;

        // Navegación a la entidad Usuario que representa al asistente que atiende al solicitante
        [Required]
        public Usuario Asistente { get; set; } = null!;

        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada componente se asocia solo una vez por eso el hash
        public ICollection<BoletaComponente> BoletaComponentes { get; set; } = new HashSet<BoletaComponente>();

        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada equipo se asocia solo una vez por eso el hash
        public ICollection<BoletaEquipo> BoletaEquipo { get; set; } = new HashSet<BoletaEquipo>();
    }
}
