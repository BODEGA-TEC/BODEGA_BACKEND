using Sibe.API.Models.Inventario;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Boletas
{
    public class BoletaEquipo
    {
        [Required]
        public int BoletaId { get; set; }

        [Required]
        public int EquipoId { get; set; }

        public string? Observaciones { get; set; }

        // Navegación a la entidad Boleta
        [ForeignKey("BoletaId")]
        public Boleta? Boleta { get; set; }

        // Navegación a la entidad Equipo
        [ForeignKey("EquipoId")]
        public Equipo? Equipo { get; set; }  
    }
}
