using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EstadoService
{
    public interface IEstadoService
    {
        /* CONTROLLERS */

        Task<ServiceResponse<Estado>> Create(string estado);
        Task<ServiceResponse<List<Estado>>> ReadAll();

        Task<ServiceResponse<Estado>> Update(int id, string estado);
        Task<ServiceResponse<object>> Delete(int id);


        /* USO INTERNO API */
        Task<Estado> FetchById(int id);
        Task<Estado> FetchByNombre(string nombre);
    }
}
