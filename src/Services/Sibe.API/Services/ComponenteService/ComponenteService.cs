using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Models;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Services.ComponenteService
{
    public class ComponenteService : IComponenteService
    {
        // Variables gloables
        private readonly DataContext _context;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;
        private readonly ComponenteServiceMessages _message;

        // Clase interna para gestionar los mensajes
        private class ComponenteServiceMessages
        {
            public readonly string NotFound = "Componente no encontrado.";
            public readonly string CreateSuccess = "Componente creado con éxito.";
            public readonly string ReadSuccess = "Componente(s) recuperado(s) con éxito.";
            public readonly string Empty = "No se han registrado componentes.";
            public readonly string UpdatedSuccess = "Componente actualizado con éxito.";
            public readonly string DeletedSuccess = "Componente eliminado con éxito.";
        }

        public ComponenteService(DataContext context, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _context = context;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
            _message = new ComponenteServiceMessages();
        }

        public async Task<ServiceResponse<Componente>> Create(CreateComponenteDto componenteDto)
        {
            var response = new ServiceResponse<Componente>();

            try
            {
                // Recuperar categorias
                var categoriaResponse = await _categoriaService.ReadById(componenteDto.CategoriaId);
                if (!categoriaResponse.Success)
                {
                    response.SetError(categoriaResponse.Message);
                    return response;
                }

                // Recuperar estado
                var estadoResponse = await _estadoService.ReadById(componenteDto.EstadoId);
                if (!estadoResponse.Success)
                {
                    response.SetError(estadoResponse.Message);
                    return response;
                }

                // Crear componente
                var componente = new Componente
                {
                    Activo = componenteDto.Activo,
                    Categoria = categoriaResponse.Data,
                    Estado = estadoResponse.Data,
                    Descripcion = componenteDto.Descripcion.ToUpper(),
                    Observaciones = componenteDto.Observaciones
                };

                // Agregar componente
                _context.Componentes.Add(componente);
                await _context.SaveChangesAsync();

                response.Data = componente;
                response.SetSuccess(_message.CreateSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Componente>>> ReadAll()
        {
            var response = new ServiceResponse<List<Componente>>();

            try
            {
                // Recuperar componentes
                var componentes = await _context.Componentes
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .ToListAsync() ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = componentes;
                string message = componentes.Count == 0
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

        public async Task<ServiceResponse<Componente>> ReadById(int id)
        {
            var response = new ServiceResponse<Componente>();

            try
            {
                // Recuperar componente
                var componente = await _context.Componentes
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .FirstOrDefaultAsync(e => e.Id == id)
                    ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = componente;
                response.SetSuccess(_message.ReadSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Componente>> Update(int id, UpdateComponenteDto componenteDto)
        {
            var response = new ServiceResponse<Componente>();

            try
            {
                // Recuperar componente
                var target = await _context.Componentes
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Actualizar componente | Solamente datos que no son null
                target.Activo = componenteDto.Activo ?? target.Activo;
                target.Descripcion = componenteDto.Descripcion ?? target.Descripcion.ToUpper();
                target.Observaciones = componenteDto.Observaciones ?? target.Observaciones;

                // Actualizar componente
                if (componenteDto.CategoriaId.HasValue)
                {
                    var categoriaResponse = await _categoriaService.ReadById((int)componenteDto.CategoriaId);
                    if (!categoriaResponse.Success)
                    {
                        response.SetError(categoriaResponse.Message);
                        return response;
                    }
                    target.Categoria = categoriaResponse.Data;
                }

                // Actualizar estado
                if (componenteDto.EstadoId.HasValue)
                {
                    var estadoResponse = await _estadoService.ReadById((int)componenteDto.EstadoId);
                    if (!estadoResponse.Success)
                    {
                        response.SetError(estadoResponse.Message);
                        return response;
                    }
                    target.Estado = estadoResponse.Data;
                }

                // Actualizar componente
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
                // Recuperar componente
                var componente = await _context.Componentes
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Eliminar componente
                _context.Componentes.Remove(componente);
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