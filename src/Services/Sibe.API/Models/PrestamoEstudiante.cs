using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class PrestamoEstudiante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public TipoComprobantePrestamo Tipo { get; set; }

        [Required]
        public string Descripcion { get; set; } = null!;

        [Required]
        public Usuario Asistente { get; set; } = null!;

        [Required]
        public string EstudianteCarne { get; set; } = null!;

        // Navegación a la entidad Profesor que representa al profesor que autoriza poder retirar componentes
        public Profesor? ProfesorAutorizador { get; set; }

        public List<Componente> Componentes { get; set; } = new List<Componente>();

        public List<Equipo> Equipo { get; set; } = new List<Equipo>();
    }
}