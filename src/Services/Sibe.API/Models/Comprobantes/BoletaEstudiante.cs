using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Comprobantes
{
    public class BoletaEstudiante : Boleta
    {
        [Required]
        public string Carne { get; set; } = null!;

        public int? ProfesorAutorizadorId { get; set; } // Agregamos ForeignKey para ProfesorAutorizadorId

        // Navegación a la entidad Profesor que representa al profesor que autoriza poder retirar componentes
        [ForeignKey("ProfesorAutorizadorId")]
        public Profesor? ProfesorAutorizador { get; set; }


    }

}