using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EquipoService
{
    public interface IEquipoService
    {
        Task<ServiceResponse<ReadEquipoDto>> Create(CreateEquipoDto equipo);
        Task<ServiceResponse<List<ReadEquipoDto>>> ReadAll();
        Task<ServiceResponse<Equipo>> ReadById(int id);
        Task<ServiceResponse<ReadEquipoDto>> Update(int id, UpdateEquipoDto equipo);
        Task<ServiceResponse<object>> Delete(int id);
        //Task<ServiceResponse<object>> DeleteTemporal(int id);
        //Task<ServiceResponse<string>> GetBarcode(int id);
    }
}
