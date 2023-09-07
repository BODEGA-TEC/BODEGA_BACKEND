using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Services.CategoriaService;

namespace Sibe.API.Services.EquipoService
{
    public class EquipoService : IEquipoService
    {
        private readonly DataContext _context;
        private readonly ICategoriaService _categoriaService;

        private const string ErrorPrefix = "[ERROR] - ";

        public EquipoService(DataContext context, ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
            _context = context;
        }

        public async Task<ServiceResponse<List<Equipo>>> ReadAll()
        {
            var response = new ServiceResponse<List<Equipo>>();

            try
            {
                // Lógica para obtener todos los equipos con entidades relacionadas
                var equipo = await _context.Equipo
                    .Include(e => e.Categoria) // Incluye la entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye la entidad relacionada Estado
                    .ToListAsync() ?? throw new Exception("Equipo no encontrado.");

                response.Data = equipo;
                response.Success = true;
                response.Message = equipo.Count == 0 
                    ? "No se ha registrado equipo." : "Equipos recuperados con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.Message);
            }
            return response;
        }


        public async Task<ServiceResponse<Equipo>> ReadById(int id)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Lógica para obtener un equipo por su ID incluyendo las entidades relacionadas
                var equipo = await _context.Equipo
                    .Include(e => e.Categoria) // Incluye la entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye la entidad relacionada Estado
                    .FirstOrDefaultAsync(e => e.Id == id)
                    ?? throw new Exception("Equipo no encontrado.");

                response.Data = equipo;
                response.Success = true;
                response.Message = "Equipo recuperado con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.Message);
            }
            return response;
        }


        public async Task<ServiceResponse<Equipo>> Create(CreateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Obtener la categoría por ID fuera del bloque de transacción
                // a través del servicio de categorías (mantenibilidad)
                var categoriaResponse = await _categoriaService.ReadById(equipoDto.CategoriaId);
                if (!categoriaResponse.Success)
                {
                    response.SetError(categoriaResponse.Message);
                    return response;
                }

                // Obtener el estado
                var estado = _context.Estados.FirstOrDefault(e => e.Id == equipoDto.EstadoId);
                if (estado == null)
                {
                    response.SetError("Estado no encontrado.");
                    return response;
                }

                // Crear un nuevo equipo
                var equipo = new Equipo
                {
                    Activo = equipoDto.Activo,
                    Serie = equipoDto.Serie,
                    Categoria = categoriaResponse.Data,
                    Estado = estado,
                    Descripcion = equipoDto.Descripcion,
                    Marca = equipoDto.Marca,
                    Modelo = equipoDto.Modelo,
                    Observaciones = equipoDto.Observaciones
                };

                // Agregar el nuevo equipo a la base de datos
                _context.Equipo.Add(equipo);
                await _context.SaveChangesAsync();

                response.Data = equipo;
                response.Success = true;
                response.Message = "Equipo creado con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }
            return response;
        }


        public async Task<ServiceResponse<Equipo>> Update(int id, UpdateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Lógica para actualizar un equipo existente por su ID
                var equipo = await _context.Equipo.FindAsync(id)
                    ?? throw new Exception("Equipo no encontrado.");

                // Actualizar los datos solo si no son null
                equipo.Activo = equipoDto.Activo ?? equipo.Activo;
                equipo.Serie = equipoDto.Serie ?? equipo.Serie;
                equipo.Descripcion = equipoDto.Descripcion ?? equipo.Descripcion;
                equipo.Marca = equipoDto.Marca ?? equipo.Marca;
                equipo.Modelo = equipoDto.Modelo ?? equipo.Modelo;
                equipo.Observaciones = equipoDto.Observaciones ?? equipo.Observaciones;

                if (equipoDto.CategoriaId.HasValue) {
                    var categoriaResponse = await _categoriaService.ReadById((int)equipoDto.CategoriaId);
                    if (!categoriaResponse.Success)
                    {
                        response.SetError(categoriaResponse.Message);
                        return response;
                    }
                    equipo.Categoria = categoriaResponse.Data;
                }
                
                if (equipoDto.EstadoId.HasValue) {
                    // Obtener el estado
                    var estado = _context.Estados.FirstOrDefault(e => e.Id == equipoDto.EstadoId);
                    if (estado == null)
                    {
                        response.SetError("Estado no encontrado.");
                        return response;
                    }
                    equipo.Estado = estado;
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                response.Data = equipo;
                response.Success = true;
                response.Message = "Equipo actualizado con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }
            return response;
        }


        public async Task<ServiceResponse<object>> Delete(int id)
        {
            var response = new ServiceResponse<object>();

            // Iniciar transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Lógica para eliminar un equipo por su ID
                var equipo = await _context.Equipo.FindAsync(id)
                    ?? throw new Exception("Equipo no encontrado.");

                // Realizar la eliminación en la base de datos
                _context.Equipo.Remove(equipo);
                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                response.Success = true;
                response.Message = "Equipo eliminado con éxito.";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, puedes deshacer la transacción
                await transaction.RollbackAsync();

                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }
            return response;
        }
    }
}
