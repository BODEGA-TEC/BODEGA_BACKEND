using Sibe.API.Models.Inventario;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Boletas
{
    /* seguimiento preciso de la cantidad prestada de cada tipo de componente en cada boleta de préstamo */
    public class BoletaComponente
    {
        [Required]
        public int BoletaId { get; set; }

        [Required]
        public int ComponenteId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public string? Observaciones { get; set; }

        // Navegación a la entidad Boleta
        [ForeignKey("BoletaId")]
        public Boleta? Boleta { get; set; }

        // Navegación a la entidad Componente
        [ForeignKey("ComponenteId")]
        public Componente? Componente { get; set; }
    }
}
