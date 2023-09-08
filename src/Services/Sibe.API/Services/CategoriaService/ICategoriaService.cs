using System.Collections.Generic;
using System.Threading.Tasks;
using Sibe.API.Models; // Asegúrate de importar el espacio de nombres correcto.
using Sibe.API.Models.Enums;

namespace Sibe.API.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<ServiceResponse<Categoria>> Create(Categoria categoria);
        Task<ServiceResponse<List<Categoria>>> ReadAll();
        Task<ServiceResponse<Categoria>> ReadById(int id);
        Task<ServiceResponse<List<Categoria>>> ReadByTipoCategoria(TipoCategoria tipo);
        Task<ServiceResponse<Categoria>> Update(int id, Categoria categoria);
        Task<ServiceResponse<object>> Delete(int id);
    }
}