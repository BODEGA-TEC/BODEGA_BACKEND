namespace Sibe.API.Data.Dtos.Componente
{
    public class UpdateComponenteDto
    {
        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }

        public string? Descripcion { get; set; }

        public string? ActivoBodega { get; set; }

        public string? ActivoTec { get; set; }

        public int? Cantidad { get; set; }

        public string Observaciones { get; set; } = string.Empty;

    }
}
