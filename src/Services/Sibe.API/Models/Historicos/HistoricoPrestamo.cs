using System.ComponentModel.DataAnnotations;
using Sibe.API.Models.Comprobantes;

namespace Sibe.API.Models.Historicos
{
    public class HistoricoPrestamo
    {
        [Key]
        public int Id { get; set; }

        // Referencia al comprobante de préstamo de estudiante (nullable)
        public PrestamoEstudiante? PrestamoEstudiante { get; set; } 

        // Referencia al comprobante de préstamo de profesor (nullable)
        public PrestamoProfesor? PrestamoProfesor { get; set; } 
    }
}
