using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EquipoService
{
    public interface IEquipoService
    {
        Task<ServiceResponse<List<Equipo>>> Create(CreateEquipoDto equipo);
        Task<ServiceResponse<List<Equipo>>> ReadAll();
        Task<ServiceResponse<Equipo>> ReadById(int id);
        Task<ServiceResponse<List<Equipo>>> Update(int id, UpdateEquipoDto equipo);
        Task<ServiceResponse<List<Equipo>>> Delete(int id);
        Task<ServiceResponse<List<Equipo>>> DeleteTemporal(int id);
        Task<ServiceResponse<string>> GetBarcode(int id);
    }
}
