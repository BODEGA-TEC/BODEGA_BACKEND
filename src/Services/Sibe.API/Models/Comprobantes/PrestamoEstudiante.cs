using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Comprobantes
{
    public class PrestamoEstudiante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = TimeZoneHelper.Now();

        [Required]
        public TipoComprobantePrestamo Tipo { get; set; }

        [Required]
        public string Descripcion { get; set; } = null!;

        // Navegación a la entidad Usuario que representa al asistente que atiende al solicitante
        [Required]
        public Usuario Asistente { get; set; } = null!;

        [Required]
        public string EstudianteCarne { get; set; } = null!;

        // Navegación a la entidad Profesor que representa al profesor que autoriza poder retirar componentes
        public Profesor? ProfesorAutorizador { get; set; }

        // Navegación a la lista de componentes asociados a este comprobante de préstamo
        public List<Componente> Componentes { get; set; } = new List<Componente>();

        // Navegación a la lista de equipos asociados a este comprobante de préstamo
        public List<Equipo> Equipo { get; set; } = new List<Equipo>();
    }
}