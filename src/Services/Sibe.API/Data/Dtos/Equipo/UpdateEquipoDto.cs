namespace Sibe.API.Data.Dtos.Equipo
{
    public class UpdateEquipoDto
    {

        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }

        public string? Descripcion { get; set; }

        public string? ActivoBodega { get; set; }

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? ActivoTec { get; set; }

        public string? Serie { get; set; }

        public string? Observaciones { get; set; }
    }
}
