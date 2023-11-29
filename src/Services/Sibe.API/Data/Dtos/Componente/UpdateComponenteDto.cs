using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Componente
{
    public class UpdateComponenteDto
    {
        public int? CategoriaId { get; set; }

        public string? Descripcion { get; set; }

        public string? ActivoTec { get; set; }

        public int? CantidadTotal { get; set; } = null;

        public int? CantidadDisponible { get; set; } = null;

        public Condicion? Condicion { get; set; }

        public string? Estante { get; set; }

        public string? NoParte { get; set; }

        public string Observaciones { get; set; } = string.Empty;

    }
}


