using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Comprobantes
{
    public class BoletaPrestamoProfesor : BoletaPrestamo
    {
        // Navegación a la entidad Profesor que representa al profesor que solicita componentes
        [Required]
        public Profesor Profesor { get; set; } = null!; // Profesor que realiza el préstamo

        [ForeignKey("Profesor")]
        public int ProfesorId { get; set; } // Agregamos ForeignKey para ProfesorId
    }
}
