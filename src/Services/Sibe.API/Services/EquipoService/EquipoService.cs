using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;

        public EquipoService(IConfiguration configuration, DataContext context, IMapper mapper, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _messages = configuration.GetSection("EquipoService");
            _context = context;
            _mapper = mapper;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
        }

        private async Task IsActivoTecInUse(string? activoTec)
        {
            if (activoTec != null && await _context.Equipo.AnyAsync(e => e.ActivoTec == activoTec))
                throw new Exception(_messages["ActivoTecInUse"]);
        }


        public async Task<Equipo> FetchById(int id)
        {
            // Recuperar equipo
            return await _context.Equipo
                .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                .FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new Exception(_messages["NotFound"]);
        }


        public async Task<ServiceResponse<ReadEquipoDto>> Create(CreateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<ReadEquipoDto>();

            try
            {
                // Si el activo tec ya se encuentra registrado
                await IsActivoTecInUse(equipoDto.ActivoTec);

                // Recuperar categoria
                var categoria = await _categoriaService.FetchById(equipoDto.CategoriaId);

                // Recuperar estado
                var estado = await _estadoService.FetchById(equipoDto.EstadoId);

                // Recuperar el id del equipo a insertar
                int scope_identity = _context.Equipo.Any() ? _context.Equipo.Max(c => c.Id) + 1 : 1;

                // Crear equipo
                var equipo = new Equipo
                {
                    Categoria = categoria,
                    Estado = estado,
                    Descripcion = equipoDto.Descripcion.ToUpper(),
                    Condicion = equipoDto.Condicion,
                    Estante = equipoDto.Estante.ToUpper(),
                    Marca = equipoDto.Marca,
                    Modelo = equipoDto.Modelo,
                    ActivoBodega = Utils.UniqueIdentifierHelper.GenerateIdentifier("BE", scope_identity, 6),
                    ActivoTec = equipoDto.ActivoTec,
                    Serie = equipoDto.Serie,
                    Observaciones = equipoDto.Observaciones
                };

                // Agregar equipo y registro histórico
                _context.Equipo.Add(equipo);

                // Map a Dto
                ReadEquipoDto entityDto = _mapper.Map<ReadEquipoDto>(equipo);

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], entityDto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<ReadEquipoDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadEquipoDto>>();

            try
            {
                // Recuperar equipo
                var equipo = await _context.Equipo
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

                // Map a Dto
                List<ReadEquipoDto> equipoDto = _mapper.Map<List<ReadEquipoDto>>(equipo);

                // Configurar respuesta
                string? message = equipoDto.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];

                // Configurar respuesta
                response.SetSuccess(message, equipoDto);
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
                var equipo = await FetchById(id);

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

        public async Task<ServiceResponse<ReadEquipoDto>> Update(int id, UpdateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<ReadEquipoDto>();

            try
            {
                // Recuperar equipo
                var target = await FetchById(id);

                // Actualizar equipo | Solamente datos que no son null
                target.Descripcion = equipoDto.Descripcion ?? target.Descripcion.ToUpper();
                target.Condicion = equipoDto.Condicion ?? target.Condicion;
                target.Estante = equipoDto.Estante ?? target.Estante.ToUpper();
                target.Marca = equipoDto.Marca ?? target.Marca;
                target.Modelo = equipoDto.Modelo ?? target.Modelo;
                target.ActivoTec = equipoDto.ActivoTec ?? target.ActivoTec;
                target.Serie = equipoDto.Serie ?? target.Serie;
                target.Observaciones = equipoDto.Observaciones ?? target.Observaciones;

                // Actualizar categoría si se proporciona un ID válido
                target.Categoria = equipoDto.CategoriaId.HasValue
                    ? await _categoriaService.FetchById((int)equipoDto.CategoriaId)
                    : target.Categoria;

                // Actualizar estado si se proporciona un ID válido
                target.Estado = equipoDto.EstadoId.HasValue
                    ? await _estadoService.FetchById((int)equipoDto.EstadoId)
                    : target.Estado;

                // Actualizar equipo y agregar registro histórico
                await _context.SaveChangesAsync();

                // Map a Dto
                ReadEquipoDto entityDto = _mapper.Map<ReadEquipoDto>(target);

                // Configurar respuesta
                response.SetSuccess(_messages["UpdatedSuccess"], entityDto);
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
                // Recuperar equipo
                var equipo = await FetchById(id);

                // Eliminar equipo
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