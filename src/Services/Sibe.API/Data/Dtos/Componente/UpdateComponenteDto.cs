using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Componente
{
    public class UpdateComponenteDto
    {
        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }

        public string? Descripcion { get; set; }

        public string? ActivoTec { get; set; }

        public int? Cantidad { get; set; } = null;
        
        public Condicion? Condicion { get; set; }

        public string? Estante { get; set; }

        public string? Modelo { get; set; }

        public string Observaciones { get; set; } = string.Empty;

    }
}


