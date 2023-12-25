using Sibe.API.Models.Boletas;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Historicos;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Equipo
{
    public class ReadEquipoDto
    {
        public int Id { get; set; }

        public string Categoria { get; set; } = string.Empty!;

        public string Estado { get; set; } = string.Empty!;

        public DateTime FechaRegistro { get; set; }

        public string Descripcion { get; set; } = string.Empty!;

        public Condicion Condicion { get; set; }

        public string Estante { get; set; } = string.Empty!;

        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public string ActivoBodega { get; set; } = string.Empty!;

        public string ActivoTec { get; set; } = string.Empty;

        public string Serie { get; set; } = string.Empty;

        public string Observaciones { get; set; } = string.Empty;
    }
}

