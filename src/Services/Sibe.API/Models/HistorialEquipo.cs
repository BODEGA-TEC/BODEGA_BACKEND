using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class HistorialEquipo
    {
        public int Id { get; set; }

        [Required]
        public Equipo Equipo { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public Estado EstadoActual { get; set; } = null!;

        [Required]
        public Estado EstadoNuevo { get; set; } = null!;

        [Required]
        public string Comentario { get; set; } = null!;

        public ComprobantePrestamo? ComprobantePrestamo { get; set; } // ID del comprobante de préstamo (nullable)
    }
}
