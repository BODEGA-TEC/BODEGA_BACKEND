using Sibe.API.Models.Boletas;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sibe.API.Models.Historicos
{
    public class HistoricoComponente
    {
        public int Id { get; set; }

        // Navegación a la entidad Componente que representa el equipo asociado al registro histórico
        [Required]
        [JsonIgnore]    // Se omite al serializar para evitar ciclos de referencia
        public Componente Componente { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; } = TimeZoneHelper.Now();

        [Required]
        public string Detalle { get; set; } = null!;

        [Required]
        public int CantidadModificada { get; set; }  // Cantidad de componentes modificados en el inventario (positivos o negativos)

        public int CantidadDisponible { get; set; } // Cantidad de componentes después del prestamo, retiro o adiccion.

        // Navegación a la entidad Boleta que almacena el Id de la boleta correspondiente.
        public Boleta? Comprobante { get; set; } // En caso de que haya prestamo
    }
}
