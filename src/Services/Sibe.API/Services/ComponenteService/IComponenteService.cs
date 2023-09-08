using Sibe.API.Models;

namespace Sibe.API.Services.ComponenteService
{
    public interface IComponenteService
    {
        Task<ServiceResponse<Componente>> Create(Componente componente); // Crear un nuevo componente
        Task<ServiceResponse<List<Componente>>> ReadAll(); // Leer todos los componentes
        Task<ServiceResponse<Componente>> ReadById(int id); // Leer un componente por su ID
        Task<ServiceResponse<Componente>> Update(int id, Componente componente); // Actualizar un equipo existente
        Task<ServiceResponse<object>> Delete(int id); // Eliminar un componente por su ID -  no devuelve datos
    }
}
