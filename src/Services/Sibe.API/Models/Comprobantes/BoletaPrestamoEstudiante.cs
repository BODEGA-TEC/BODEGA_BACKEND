using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Comprobantes
{
    public class BoletaPrestamoEstudiante : BoletaPrestamo
    {
        [Required]
        public string Carne { get; set; } = null!;

        // Navegación a la entidad Profesor que representa al profesor que autoriza poder retirar componentes
        public Profesor? ProfesorAutorizador { get; set; }

        [ForeignKey("ProfesorAutorizador")]
        public int? ProfesorAutorizadorId { get; set; } // Agregamos ForeignKey para ProfesorAutorizadorId
    }

}