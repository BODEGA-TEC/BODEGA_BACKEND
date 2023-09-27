using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Services.EquipoService
{
    public class EquipoService : IEquipoService
    {
        // Variables gloables
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;

        public EquipoService(IConfiguration configuration, DataContext context, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _messages = configuration.GetSection("EquipoService");
            _context = context;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
        }

        private async Task IsActivoTecInUse(string? activoTec)
        {
            if (activoTec != null && await _context.Equipo.AnyAsync(e => e.ActivoTec == activoTec))
                throw new Exception(_messages["ActivoTecInUse"]);
        }

        public async Task<ServiceResponse<Equipo>> Create(CreateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Si el activo tec ya se encuentra registrado
                await IsActivoTecInUse(equipoDto.ActivoTec);

                // Recuperar categorias
                var categoriaResponse = await _categoriaService.ReadById(equipoDto.CategoriaId);
                if (!categoriaResponse.Success || categoriaResponse.Data == null)
                {
                    response.SetError(categoriaResponse.Message);
                    return response;
                }

                // Recuperar estado
                var estadoResponse = await _estadoService.ReadById(equipoDto.EstadoId);
                if (!estadoResponse.Success || estadoResponse.Data == null)
                {
                    response.SetError(estadoResponse.Message);
                    return response;
                }

                // Crear equipo
                var equipo = new Equipo
                {
                    ActivoBodega = equipoDto.ActivoBodega,
                    ActivoTec = equipoDto.ActivoTec,
                    Serie = equipoDto.Serie,
                    Categoria = categoriaResponse.Data,
                    Estado = estadoResponse.Data,
                    Descripcion = equipoDto.Descripcion.ToUpper(),
                    Marca = equipoDto.Marca,
                    Modelo = equipoDto.Modelo,
                    Observaciones = equipoDto.Observaciones
                };

                // Agregar equipo
                _context.Equipo.Add(equipo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], equipo);
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
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                string? message = equipo.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];
                response.SetSuccess(message, equipo);
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
                    ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], equipo);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Equipo>> Update(int id, UpdateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<Equipo>();

            try
            {
                // Recuperar equipo
                var target = await _context.Equipo
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Actualizar equipo | Solamente datos que no son null
                target.ActivoBodega = equipoDto.ActivoBodega ?? target.ActivoBodega;
                target.ActivoTec = equipoDto.ActivoTec ?? target.ActivoTec;
                target.Serie = equipoDto.Serie ?? target.Serie;
                target.Descripcion = equipoDto.Descripcion ?? target.Descripcion.ToUpper();
                target.Marca = equipoDto.Marca ?? target.Marca;
                target.Modelo = equipoDto.Modelo ?? target.Modelo;
                target.Observaciones = equipoDto.Observaciones ?? target.Observaciones;

                // Actualizar categoria
                if (equipoDto.CategoriaId.HasValue)
                {
                    var categoriaResponse = await _categoriaService.ReadById((int)equipoDto.CategoriaId);
                    if (!categoriaResponse.Success || categoriaResponse.Data == null)
                    {
                        response.SetError(categoriaResponse.Message);
                        return response;
                    }
                    target.Categoria = categoriaResponse.Data;
                }

                // Actualizar estado
                if (equipoDto.EstadoId.HasValue)
                {
                    var estadoResponse = await _estadoService.ReadById((int)equipoDto.EstadoId);
                    if (!estadoResponse.Success || estadoResponse.Data == null)
                    {
                        response.SetError(estadoResponse.Message);
                        return response;
                    }
                    target.Estado = estadoResponse.Data;
                }

                // Actualizar equipo
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
                // Recuperar categoría
                var equipo = await _context.Equipo
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Eliminar categoría
                _context.Equipo.Remove(equipo);
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