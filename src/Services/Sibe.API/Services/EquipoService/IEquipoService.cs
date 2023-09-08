using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;

namespace Sibe.API.Services.EquipoService
{
    public interface IEquipoService
    {
        Task<ServiceResponse<Equipo>> Create(CreateEquipoDto equipo); // Crear un nuevo equipo
        Task<ServiceResponse<List<Equipo>>> ReadAll(); // Leer todos los equipos
        Task<ServiceResponse<Equipo>> ReadById(int id); // Leer un equipo por su ID
        Task<ServiceResponse<Equipo>> Update(int id, UpdateEquipoDto equipo); // Actualizar un equipo existente
        Task<ServiceResponse<object>> Delete(int id); // Eliminar un equipo por su ID -  no devuelve datos
    }
}
