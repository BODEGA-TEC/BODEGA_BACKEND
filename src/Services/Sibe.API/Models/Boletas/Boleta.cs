using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Comprobantes
{
    public class Boleta
    {
        [Key]
        public int Id { get; set; }

        public required DateTime FechaEmision { get; set; } = TimeZoneHelper.Now();

        public required TipoBoleta Tipo { get; set; }

        public required string Detalle { get; set; } = null!; // Puede ser el curso para el que se solicita

        public required Usuario Asistente { get; set; } = null!; // Navegación a la entidad Usuario que representa al asistente que atiende al solicitante

        public string? CarneSolicitante { get; set; }

        public Usuario? Aprobador { get; set; } // Deberia darse la opcion de seleccionar a un profesor y enviar el Id

        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada componente se asocia solo una vez por eso el hash
        public ICollection<BoletaComponente> BoletaComponentes { get; set; } = new HashSet<BoletaComponente>();

        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada equipo se asocia solo una vez por eso el hash
        public ICollection<BoletaEquipo> BoletaEquipo { get; set; } = new HashSet<BoletaEquipo>();
    }
}
