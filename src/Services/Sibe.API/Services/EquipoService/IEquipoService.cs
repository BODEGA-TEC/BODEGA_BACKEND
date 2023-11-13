using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EquipoService
{
    public interface IEquipoService
    {
        Task<ServiceResponse<ReadEquipoDto>> Create(CreateEquipoDto equipoDto);
        Task<ServiceResponse<object>> CreateMultiple(List<CreateEquipoSpecialDto> equiposDtoList);
        Task<ServiceResponse<List<ReadEquipoDto>>> ReadAll();
        Task<ServiceResponse<Equipo>> ReadById(int id);
        Task<ServiceResponse<ReadEquipoDto>> Update(int id, UpdateEquipoDto equipoDto);
        Task<ServiceResponse<object>> Delete(int id);
    }
}
