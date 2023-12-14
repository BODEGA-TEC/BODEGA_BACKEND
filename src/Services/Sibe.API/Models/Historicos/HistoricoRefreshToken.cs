using Sibe.API.Models.Entidades;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Historicos
{
    public class HistoricoRefreshToken
    {
        [Key]
        public int Id { get; set; }
        public required Usuario Usuario { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }

        // Propiedad de solo lectura para la columna calculada
        public bool EsActivo => FechaExpiracion >= TimeZoneHelper.Now();
    }
}
