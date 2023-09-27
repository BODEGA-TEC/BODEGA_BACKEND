using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Services.ComponenteService
{
    public class ComponenteService : IComponenteService
    {
        // Variables gloables
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;

        public ComponenteService(IConfiguration configuration, DataContext context, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _messages = configuration.GetSection("ComponenteService");
            _context = context;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
        }


        private async Task IsActivoTecInUse(string? activoTec)
        {
            if (activoTec != null && await _context.Componente.AnyAsync(c => c.ActivoTec == activoTec))
                throw new Exception(_messages["ActivoTecInUse"]);
        }

        public async Task<ServiceResponse<Componente>> Create(CreateComponenteDto componenteDto)
        {
            var response = new ServiceResponse<Componente>();

            try
            {
                // Si el activo tec ya se encuentra registrado
                await IsActivoTecInUse(componenteDto.ActivoTec);

                // Recuperar categorias
                var categoriaResponse = await _categoriaService.ReadById(componenteDto.CategoriaId);
                if (!categoriaResponse.Success || categoriaResponse.Data == null)
                {
                    response.SetError(categoriaResponse.Message);
                    return response;
                }

                // Recuperar estado
                var estadoResponse = await _estadoService.ReadById(componenteDto.EstadoId);
                if (!estadoResponse.Success || estadoResponse.Data == null)
                {
                    response.SetError(estadoResponse.Message);
                    return response;
                }

                // Crear componente
                var componente = new Componente
                {
                    Categoria = categoriaResponse.Data,
                    Estado = estadoResponse.Data,
                    Descripcion = componenteDto.Descripcion.ToUpper(),
                    Cantidad = componenteDto.Cantidad,
                    ActivoBodega = componenteDto.ActivoBodega,
                    ActivoTec = componenteDto.ActivoTec,
                    Observaciones = componenteDto.Observaciones
                };

                // Agregar componente
                _context.Componente.Add(componente);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], componente);
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
                var componentes = await _context.Componente
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                string? message = componentes.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];
                response.SetSuccess(message, componentes);
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
                var componente = await _context.Componente
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .FirstOrDefaultAsync(e => e.Id == id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], componente);
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
                var target = await _context.Componente
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Actualizar componente | Solamente datos que no son null
                target.Descripcion = componenteDto.Descripcion ?? target.Descripcion.ToUpper();
                target.ActivoBodega = componenteDto.ActivoBodega ?? target.ActivoBodega;
                target.ActivoTec = componenteDto.ActivoTec ?? target.ActivoTec;
                target.Cantidad = componenteDto.Cantidad ?? target.Cantidad;
                target.Observaciones = componenteDto.Observaciones ?? target.Observaciones;

                // Actualizar componente
                if (componenteDto.CategoriaId.HasValue)
                {
                    var categoriaResponse = await _categoriaService.ReadById((int)componenteDto.CategoriaId);
                    if (!categoriaResponse.Success || categoriaResponse.Data == null)
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
                    if (!estadoResponse.Success || estadoResponse.Data == null)
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
                response.SetSuccess(_messages["UpdatedSuccess"], result.Data);
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
                var componente = await _context.Componente
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Eliminar componente
                _context.Componente.Remove(componente);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["DeletedSuccess"]);
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