using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Equipo
{
    public class CreateEquipoSpecialDto
    {
        
        public string Categoria { get; set; } = string.Empty!;
        public string Estado { get; set; } = string.Empty!;
        public string Descripcion { get; set; } = string.Empty!;
        public Condicion Condicion { get; set; }
        public string Estante { get; set; } = string.Empty!;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? ActivoTec { get; set; }
        public string? Serie { get; set; }
        public string? Observaciones { get; set; }
        public int Cantidad { get; set; }
    }
}
