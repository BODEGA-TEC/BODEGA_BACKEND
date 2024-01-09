using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Models;
using Sibe.API.Models.Entidades;

namespace Sibe.API.Services.AsistenteService
{
    public interface IAsistenteService
    {
        Task<Asistente> FetchByCarne(string carne);
        Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAll();
        //Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAllActivo();
        Task<ServiceResponse<ReadAsistenteDto>> ReadByCarne(string carne);
        Task<ServiceResponse<object>> RegisterAsistentes(List<RegisterAsistenteDto> asistentesDto);
        Task<ServiceResponse<object>> RegisterAsistenteCredentials(string carne, string pin, string fingerprint);
        Task<ServiceResponse<ReadAsistenteDto>> AuthenticateAsistente(string fingerprint);
        Task<ServiceResponse<ReadAsistenteDto>> AuthenticateAsistente(string carne, string pin);
    }
}