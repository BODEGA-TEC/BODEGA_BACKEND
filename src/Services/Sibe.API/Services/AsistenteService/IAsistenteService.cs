using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models;

namespace Sibe.API.Services.AsistenteService
{
    public interface IAsistenteService
    {

        Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAll();
        Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAllActivo();
        Task<ServiceResponse<ReadAsistenteDto>> ReadByCarne(string carne);
        Task<ServiceResponse<ReadAsistenteDto>> ReadByHuellaDigital(string data);
        Task<ServiceResponse<object>> RegisterAsistentes(List<RegisterUsuarioDto> asistentesDto);

        // Implementación para asignar la huella digital al asistente con el ID proporcionado
        // y utilizando los datos de la huella digital (fingerprintData).
        Task<ServiceResponse<object>> RegisterHuellaDigitalAsistente(int asistenteId, string data);

    }
}
