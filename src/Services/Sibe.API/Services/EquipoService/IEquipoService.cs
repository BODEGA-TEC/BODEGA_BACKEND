using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EquipoService
{
    public interface IEquipoService
    {
        Task<ServiceResponse<Equipo>> Create(CreateEquipoDto equipo);
        Task<ServiceResponse<List<Equipo>>> ReadAll();
        Task<ServiceResponse<Equipo>> ReadById(int id);
        Task<ServiceResponse<Equipo>> Update(int id, UpdateEquipoDto equipo);
        Task<ServiceResponse<object>> Delete(int id);
    }
}
