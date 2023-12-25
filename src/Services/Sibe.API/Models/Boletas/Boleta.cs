using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Boletas
{
    public class Boleta
    {
        [Key]
        public int Id { get; set; }
        
        public required string Consecutivo { get; set; }
        public TipoBoleta TipoBoleta { get; set; }
        public required BoletaEstado Estado { get; set; }
        public DateTime FechaEmision { get; set; }


        public string? Aprobador { get; set; }


        public required string NombreAsistente { get; set; }
        public required string CarneAsistente { get; set; }


        public TipoSolicitante TipoSolicitante { get; set; }
        public required string NombreSolicitante { get; set; }
        public required string CorreoSolicitante { get; set; }
        public string? CarneSolicitante { get; set; }


        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada componente se asocia solo una vez por eso el hash
        public ICollection<BoletaComponente> BoletaComponentes { get; set; } = new HashSet<BoletaComponente>();

        // Navegación al hash de componentes asociados a este comprobante de préstamo - cada equipo se asocia solo una vez por eso el hash
        public ICollection<BoletaEquipo> BoletaEquipo { get; set; } = new HashSet<BoletaEquipo>();
    }
}
