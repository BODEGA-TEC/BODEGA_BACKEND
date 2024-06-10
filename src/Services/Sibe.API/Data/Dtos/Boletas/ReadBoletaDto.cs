using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Boletas
{
    public class ReadBoletaDto
    {
        public int Id { get; set; }
        public string Consecutivo { get; set; } = string.Empty;
        public TipoBoleta TipoBoleta { get; set; }
        public BoletaEstado Estado { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Aprobador { get; set; } = string.Empty;
        public string NombreAsistente { get; set; } = string.Empty;
        public string CarneAsistente { get; set; } = string.Empty;
        public required Solicitante Solicitante { get; set; } 

        // En caso de querer visualizar el equipo y demás, se solicita el pdf
    }
}

