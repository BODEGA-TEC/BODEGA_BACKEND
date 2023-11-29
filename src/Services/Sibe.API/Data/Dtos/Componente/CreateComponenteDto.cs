using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Componente
{
    public class CreateComponenteDto
    {
        public int CategoriaId { get; set; }
        
        public string Descripcion { get; set; } = string.Empty!;

        public int CantidadTotal { get; set; }

        public int CantidadDisponible { get; set; }

        public Condicion Condicion { get; set; }

        public string Estante { get; set; } = string.Empty!;

        public string? NoParte { get; set; }

        public string? ActivoTec { get; set; }

        public string? Observaciones { get; set; }
    }
}