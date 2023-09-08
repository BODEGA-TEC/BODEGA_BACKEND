using Sibe.API.Models;

namespace Sibe.API.Services.EstadoService
{
    public interface IEstadoService
    {
        Task<ServiceResponse<Estado>> Create(string estado);
        Task<ServiceResponse<List<Estado>>> ReadAll();
        Task<ServiceResponse<Estado>> ReadById(int id);
        Task<ServiceResponse<Estado>> Update(int id, string estado);
        Task<ServiceResponse<object>> Delete(int id);
    }
}
