using System.ComponentModel.DataAnnotations;
using Sibe.API.Models.Comprobantes;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;

namespace Sibe.API.Models.Historicos
{
    public class HistoricoEquipo
    {
        public int Id { get; set; }

        // Navegación a la entidad Equipo que representa el equipo asociado al registro histórico
        [Required]
        public Equipo Equipo { get; set; } = null!;

        // Navegación a la entidad Estado que representa el estado al que pasó el equipo en el registro histórico
        [Required]
        public Estado Estado { get; set; } = null!; // Estado al que pasó el equipo

        [Required]
        public DateTime Fecha { get; set; } = TimeZoneHelper.Now();

        [Required]
        public string Detalle { get; set; } = null!;

        // Navegación a la entidad Boleta que almacena el Id de la boleta correspondiente.
        public BoletaPrestamo? Comprobante { get; set; } // En caso de que haya prestamo
    }
}

