using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Asistente
{
    public class ReadAsistenteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Carne { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
