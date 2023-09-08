using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Services.EquipoService
{
    public class EquipoService : IEquipoService
    {
        // Variables gloables
        private readonly DataContext _context;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;
        private readonly EquipoServiceMessages _message;

        // Clase interna para gestionar los mensajes
        private class EquipoServiceMessages
        {
            public readonly string NotFound = "Equipo no encontrado.";
            public readonly string CreateSuccess = "Equipo creado con éxito.";
            public readonly string ReadSuccess = "Equipo recuperado con éxito.";
            public readonly string Empty = "No se ha registrado equipo.";
            public readonly string UpdatedSuccess = "Equipo actualizado con éxito.";
            public readonly string DeletedSuccess = "Equipo eliminado con éxito.";
        }

        public EquipoService(DataContext context, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _context = context;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
            _message = new EquipoServiceMessages();
        }

        public async Task<ServiceResponse<Equipo>> Create(CreateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Recuperar categorias
                var categoriaResponse = await _categoriaService.ReadById(equipoDto.CategoriaId);
                if (!categoriaResponse.Success)
                {
                    response.SetError(categoriaResponse.Message);
                    return response;
                }

                // Recuperar estado
                var estadoResponse = await _estadoService.ReadById(equipoDto.EstadoId);
                if (!estadoResponse.Success)
                {
                    response.SetError(estadoResponse.Message);
                    return response;
                }

                // Crear equipo
                var equipo = new Equipo
                {
                    Activo = equipoDto.Activo,
                    Serie = equipoDto.Serie,
                    Categoria = categoriaResponse.Data,
                    Estado = estadoResponse.Data,
                    Descripcion = equipoDto.Descripcion,
                    Marca = equipoDto.Marca,
                    Modelo = equipoDto.Modelo,
                    Observaciones = equipoDto.Observaciones
                };

                // Agregar equipo
                _context.Equipo.Add(equipo);
                await _context.SaveChangesAsync();

                response.Data = equipo;
                response.SetSuccess(_message.CreateSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Equipo>>> ReadAll()
        {
            var response = new ServiceResponse<List<Equipo>>();

            try
            {
                // Recuperar equipo
                var equipo = await _context.Equipo
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                    .ToListAsync() ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = equipo;
                string message = equipo.Count == 0
                    ? _message.Empty
                    : _message.ReadSuccess;
                response.SetSuccess(message);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Equipo>> ReadById(int id)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Recuperar equipo
                var equipo = await _context.Equipo
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                    .FirstOrDefaultAsync(e => e.Id == id)
                    ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = equipo;
                response.SetSuccess(_message.ReadSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Equipo>> Update(int id, UpdateEquipoDto equipo)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Recuperar equipo
                var target = await _context.Equipo
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Actualizar equipo | Solamente datos que no son null
                target.Activo = equipo.Activo ?? target.Activo;
                target.Serie = equipo.Serie ?? target.Serie;
                target.Descripcion = equipo.Descripcion ?? target.Descripcion;
                target.Marca = equipo.Marca ?? target.Marca;
                target.Modelo = equipo.Modelo ?? target.Modelo;
                target.Observaciones = equipo.Observaciones ?? target.Observaciones;

                // Actualizar categoria
                if (equipo.CategoriaId.HasValue)
                {
                    var categoriaResponse = await _categoriaService.ReadById((int)equipo.CategoriaId);
                    if (!categoriaResponse.Success)
                    {
                        response.SetError(categoriaResponse.Message);
                        return response;
                    }
                    target.Categoria = categoriaResponse.Data;
                }

                // Actualizar estado
                if (equipo.EstadoId.HasValue)
                {
                    var estado = await _estadoService.ReadById((int)equipo.EstadoId);
                    if (!estado.Success)
                    {
                        response.SetError(estado.Message);
                        return response;
                    }
                    target.Estado = estado.Data;
                }

                // Actualizar equipo
                await _context.SaveChangesAsync();

                // Configurar respuesta
                var result = await ReadById(id);
                response.Data = result.Data;
                response.SetSuccess(_message.UpdatedSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<object>> Delete(int id)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Recuperar categoría
                var equipo = await _context.Equipo
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Eliminar categoría
                _context.Equipo.Remove(equipo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_message.DeletedSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }
    }
}