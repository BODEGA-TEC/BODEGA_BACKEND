using Sibe.API.Models.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Comprobantes
{
    public class BoletaProfesor : Boleta
    {
        public int ProfesorId { get; set; } // Agregamos ForeignKey para ProfesorId

        // Navegación a la entidad Profesor que representa al profesor que solicita componentes
        [Required]
        [ForeignKey("ProfesorId")]
        public Profesor Profesor { get; set; } = null!; // Profesor que realiza el préstamo

    }
}
