using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.ComponenteService
{
    public interface IComponenteService
    {
        Task<ServiceResponse<List<Componente>>> Create(CreateComponenteDto componente);
        Task<ServiceResponse<List<Componente>>> ReadAll();
        Task<ServiceResponse<Componente>> ReadById(int id);
        Task<ServiceResponse<List<Componente>>> Update(int id, UpdateComponenteDto componente);
        Task<ServiceResponse<List<Componente>>> Delete(int id);
        Task<ServiceResponse<List<Componente>>> DeleteTemporal(int id);
    }
}
