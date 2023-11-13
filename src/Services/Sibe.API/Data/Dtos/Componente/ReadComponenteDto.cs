using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Componente
{
    public class ReadComponenteDto
    {
        public int Id { get; set; }

        public string Categoria { get; set; } = string.Empty!;

        public string Estado { get; set; } = string.Empty!;

        public DateTime FechaRegistro { get; set; }

        public string Descripcion { get; set; } = string.Empty!;

        public int Cantidad { get; set; }

        public Condicion Condicion { get; set; }

        public string Estante { get; set; } = string.Empty!;

        public string Modelo { get; set; } = string.Empty;

        public string ActivoBodega { get; set; } = string.Empty!;

        public string ActivoTec { get; set; } = string.Empty;

        public string Observaciones { get; set; } = string.Empty;
    }
}
