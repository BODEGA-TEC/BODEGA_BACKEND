using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Boletas
{
    public class BoletaEquipoDto
    {
        public int Id { get; set; }
        public string? Observaciones { get; set; }
    }

    public class BoletaComponenteDto
    {
        private int _cantidad;
        public int Id { get; set; }
        public int Cantidad
        {
            get => _cantidad;
            set => _cantidad = Math.Abs(value);
        }
    }

    public class CreateBoletaDto
    {
        public string? Aprobador { get; set; }
    
        public string CarneAsistente { get; set; } = string.Empty;

        public string IdSolicitante { get; set; } = string.Empty;

        public TipoSolicitante TipoSolicitante { get; set; }

        public List<BoletaEquipoDto> Equipo { get; set; } = new List<BoletaEquipoDto>();

        public List<BoletaComponenteDto> Componentes { get; set; } = new List<BoletaComponenteDto>();
    }
}
