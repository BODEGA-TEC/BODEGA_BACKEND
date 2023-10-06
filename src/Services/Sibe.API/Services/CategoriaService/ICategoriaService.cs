using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.CategoriaService
{
    public interface ICategoriaService
    {
        /* CONTROLLERS */

        Task<ServiceResponse<Categoria>> Create(Categoria categoria);
        Task<ServiceResponse<List<Categoria>>> ReadAll();
        Task<ServiceResponse<List<Categoria>>> ReadByTipoCategoria(TipoCategoria tipo);
        Task<ServiceResponse<Categoria>> Update(int id, Categoria categoria);
        Task<ServiceResponse<object>> Delete(int id);


        /* USO INTERNO API */
        Task<Categoria> FetchById(int id);
        Task<Categoria> FetchByNombre(string nombre);

    }
}