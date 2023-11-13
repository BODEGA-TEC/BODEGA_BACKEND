using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Historicos;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.EstadoService;
using System.ComponentModel;

namespace Sibe.API.Services.ComponenteService
{
    public class ComponenteService : IComponenteService
    {
        // Variables gloables
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IEstadoService _estadoService;
        private readonly ICategoriaService _categoriaService;

        public ComponenteService(IConfiguration configuration, DataContext context, IMapper mapper, IEstadoService estadoService, ICategoriaService categoriaService)
        {
            _messages = configuration.GetSection("ComponenteService");
            _context = context;
            _mapper = mapper;
            _estadoService = estadoService;
            _categoriaService = categoriaService;
        }


        //private async Task IsActivoTecInUse(string? activoTec)
        //{
        //    if (activoTec != null && await _context.Componente.AnyAsync(c => c.ActivoTec == activoTec))
        //        throw new Exception(_messages["ActivoTecInUse"]);
        //}

        public async Task<Componente> FetchById(int id)
        {
            // Recuperar equipo
            return await _context.Componente
                .Include(c => c.Categoria) // Incluye entidad relacionada Categoria
                .Include(c => c.Estado)    // Incluye entidad relacionada Estado
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new Exception(_messages["NotFound"]);
        }

        public async Task<ServiceResponse<ReadComponenteDto>> Create(CreateComponenteDto componenteDto)
        {
            var response = new ServiceResponse<ReadComponenteDto>();

            try
            {
                // Si el activo tec ya se encuentra registrado
                //await IsActivoTecInUse(componenteDto.ActivoTec);

                // Recuperar categoria
                var categoria = await _categoriaService.FetchById(componenteDto.CategoriaId);

                // Recuperar estado
                var estado = await _estadoService.FetchById(componenteDto.EstadoId);

                // Recuperar el id del componente a insertar
                int scope_identity = _context.Componente.Any() ? _context.Componente.Max(c => c.Id) + 1 : 1;

                // Crear componente
                var componente = new Componente
                {
                    Categoria = categoria,
                    Estado = estado,
                    Descripcion = componenteDto.Descripcion.ToUpper(),
                    Cantidad = componenteDto.Cantidad,
                    Condicion = componenteDto.Condicion,
                    Estante = componenteDto.Estante.ToUpper(),
                    Modelo = componenteDto.Modelo,
                    ActivoBodega = Utils.UniqueIdentifierHelper.GenerateIdentifier("BC", scope_identity, 6),
                    //ActivoTec = componenteDto.ActivoTec,
                    Observaciones = componenteDto.Observaciones
                };

                // Agregar componente
                _context.Componente.Add(componente);
                await _context.SaveChangesAsync();

                // Crear historico componentes
                var historicoComponente = new HistoricoComponente
                {
                    Componente = componente,
                    CantidadDisponible = componente.Cantidad,
                    CantidadModificada = 0, // Todos los componentes siguen disponibles.
                    Detalle = "Registrado"
                };

                // Agregar registro histórico
                _context.HistoricoComponente.Add(historicoComponente);
                await _context.SaveChangesAsync();

                // Map a Dto
                ReadComponenteDto entityDto = _mapper.Map<ReadComponenteDto>(componente);

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

        public async Task<ServiceResponse<List<ReadComponenteDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadComponenteDto>>();

            try
            {
                // Recuperar componentes
                var componentes = await _context.Componente
                    .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                    .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

                // Map a Dto
                List<ReadComponenteDto> componenteDto = _mapper.Map<List<ReadComponenteDto>>(componentes);

                // Configurar respuesta
                string? message = componenteDto.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];

                // Configurar respuesta
                response.SetSuccess(message, componenteDto);
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
                var componente = await FetchById(id);

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

        public async Task<ServiceResponse<ReadComponenteDto>> Update(int id, UpdateComponenteDto componenteDto)
        {
            var response = new ServiceResponse<ReadComponenteDto>();

            try
            {
                // Recuperar componente
                var target = await FetchById(id);

                // Para tener un historial de la diferencia de la cantidad que habia antes.
                int modificado = componenteDto.Cantidad.HasValue ? componenteDto.Cantidad.Value - target.Cantidad : 0;

                // Actualizar componente | Solamente datos que no son null
                target.Descripcion = componenteDto.Descripcion ?? target.Descripcion.ToUpper();
                //target.ActivoTec = componenteDto.ActivoTec ?? target.ActivoTec;
                target.Cantidad = componenteDto.Cantidad ?? target.Cantidad;
                target.Condicion = componenteDto.Condicion ?? target.Condicion;
                target.Estante = componenteDto.Estante ?? target.Estante.ToUpper();
                target.Modelo = componenteDto.Modelo ?? target.Modelo;
                target.Observaciones = componenteDto.Observaciones ?? target.Observaciones;

                // Actualizar categoría si se proporciona un ID válido
                target.Categoria = componenteDto.CategoriaId.HasValue
                    ? await _categoriaService.FetchById((int)componenteDto.CategoriaId)
                    : target.Categoria;

                // Actualizar estado si se proporciona un ID válido
                target.Estado = componenteDto.EstadoId.HasValue
                    ? await _estadoService.FetchById((int)componenteDto.EstadoId)
                    : target.Estado;

                // Crear historico componentes
                var historicoComponente = new HistoricoComponente
                {
                    Componente = target,
                    CantidadDisponible = target.Cantidad,
                    CantidadModificada = modificado,
                    Detalle = "Actualizado"
                };

                // Actualizar componente y agregar registro histórico
                _context.HistoricoComponente.Add(historicoComponente);
                await _context.SaveChangesAsync();

                // Map a Dto
                ReadComponenteDto entityDto = _mapper.Map<ReadComponenteDto>(target);

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
                // Recuperar componente
                var componente = await FetchById(id);

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