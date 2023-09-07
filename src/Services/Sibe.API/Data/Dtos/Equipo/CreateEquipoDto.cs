using Sibe.API.Models;
using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Equipo
{
    public class CreateEquipoDto
    {
        public string Activo { get; set; } = string.Empty!;

        public string? Serie { get; set; }

        public int CategoriaId { get; set; }

        public int EstadoId { get; set; } = 1;
        public TipoEstado TipoEstado { get; set; } = TipoEstado.BODEGA;

        public string Descripcion { get; set; } = string.Empty!;

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? Observaciones { get; set; }
    }
}
