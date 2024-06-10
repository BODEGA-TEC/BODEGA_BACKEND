using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.ComponenteService
{
    public interface IComponenteService
    {
        Task<Componente> FetchById(int id);
        Task<Componente> FetchByActivoBodega(string activoBodega);


        Task<ServiceResponse<object>> Create(CreateComponenteDto componente);
        Task<ServiceResponse<List<ReadComponenteDto>>> ReadAll();
        Task<ServiceResponse<Componente>> ReadById(int id);
        Task<ServiceResponse<ReadComponenteDto>> Update(int id, UpdateComponenteDto componente);
        Task<ServiceResponse<object>> Delete(int id);
    }
}
