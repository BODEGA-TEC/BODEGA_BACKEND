using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models
{
    public class HistorialEquipo
    {
        public int Id { get; set; }

        [Required]
        public Equipo Equipo { get; set; } = null!;
         
        [Required]
        public Estado Estado { get; set; } = null!; // Estado al que pasó el equipo

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Detalle { get; set; } = null!;

        //public int? ComprobantePrestamoId { get; set; } // ID del comprobante de préstamo (nullable)

        //[ForeignKey("ComprobantePrestamoId")]
        //public PrestamoEstudiante? PrestamoEstudiante { get; set; } // Referencia al comprobante de préstamo de estudiante (nullable)

        //[ForeignKey("ComprobantePrestamoId")]
        //public PrestamoProfesor? PrestamoProfesor { get; set; } // Referencia al comprobante de préstamo de profesor (nullable)
    }
}

