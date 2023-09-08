namespace Sibe.API.Data.Dtos.Componente
{
    public class UpdateComponenteDto
    {

        public string? Activo { get; set; }

        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }

        public string? Descripcion { get; set; }

        public string Observaciones { get; set; } = string.Empty;

    }
}
