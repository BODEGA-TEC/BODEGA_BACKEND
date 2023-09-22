using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class ComprobantePrestamo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public TipoComprobantePrestamo Tipo { get; set; }

        [Required]
        public Usuario Asistente { get; set; } = null!;

        [Required]
        public string SolicitanteId { get; set; } = null!;

        public bool EsProfesor { get; set; } = false;

        [Required]
        public string Descripcion { get; set; } = null!;
    }
}