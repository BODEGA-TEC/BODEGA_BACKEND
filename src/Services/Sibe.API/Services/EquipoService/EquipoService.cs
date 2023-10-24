using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Historicos;
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

        public async Task<List<Equipo>> FetchAll()
        {
            // Recuperar equipo
            return await _context.Equipo
                .Include(e => e.Categoria) // Incluye entidad relacionada Categoria
                .Include(e => e.Estado)    // Incluye entidad relacionada Estado
                .ToListAsync() ?? throw new Exception(_messages["NotFound"]);
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


        public async Task<ServiceResponse<List<Equipo>>> Create(CreateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<List<Equipo>>();

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
                    ActivoBodega = Utils.UniqueIdentifierHelper.GenerateIdentifier("BE", scope_identity, 6),
                    ActivoTec = equipoDto.ActivoTec,
                    Serie = equipoDto.Serie,
                    Categoria = categoria,
                    Estado = estado,
                    Descripcion = equipoDto.Descripcion.ToUpper(),
                    Marca = equipoDto.Marca,
                    Modelo = equipoDto.Modelo,
                    Observaciones = equipoDto.Observaciones
                };

                // Agregar equipo y registro histórico
                _context.Equipo.Add(equipo);

                // Accede al ID generado después de agregar el componente al contexto
                //Utils.IdentifierHelper equipo.Id;


                // Crear historico equipo
                var historicoEquipo = new HistoricoEquipo
                {
                    Equipo = equipo,
                    Estado = equipo.Estado,
                    Detalle = "Equipo creado"
                };

                // Agregar registro histórico
                _context.HistoricoEquipo.Add(historicoEquipo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], await FetchAll());
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
                var equipo = await FetchAll();

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

        public async Task<ServiceResponse<List<Equipo>>> Update(int id, UpdateEquipoDto equipoDto)
        {
            var response = new ServiceResponse<List<Equipo>>();

            try
            {
                // Recuperar equipo
                var target = await FetchById(id);

                // Actualizar equipo | Solamente datos que no son null
                target.ActivoTec = equipoDto.ActivoTec ?? target.ActivoTec;
                target.Serie = equipoDto.Serie ?? target.Serie;
                target.Descripcion = equipoDto.Descripcion ?? target.Descripcion.ToUpper();
                target.Marca = equipoDto.Marca ?? target.Marca;
                target.Modelo = equipoDto.Modelo ?? target.Modelo;
                target.Observaciones = equipoDto.Observaciones ?? target.Observaciones;

                // Actualizar categoría si se proporciona un ID válido
                target.Categoria = equipoDto.CategoriaId.HasValue
                    ? await _categoriaService.FetchById((int)equipoDto.CategoriaId)
                    : target.Categoria;

                // Actualizar estado si se proporciona un ID válido
                target.Estado = equipoDto.EstadoId.HasValue
                    ? await _estadoService.FetchById((int)equipoDto.EstadoId)
                    : target.Estado;

                // Crear historico equipo
                var historicoEquipo = new HistoricoEquipo
                {
                    Equipo = target,
                    Estado = target.Estado,
                    Detalle = "Equipo actualizado"
                };

                // Actualizar equipo y agregar registro histórico
                _context.HistoricoEquipo.Add(historicoEquipo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["UpdatedSuccess"], await FetchAll());
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Equipo>>> Delete(int id)
        {
            var response = new ServiceResponse<List<Equipo>>();

            try
            {
                // Recuperar equipo
                var target = await FetchById(id);

                // Estado a Retirado
                target.Estado = await _estadoService.FetchByNombre("RETIRADO");

                // Crear historico equipo
                var historicoEquipo = new HistoricoEquipo
                {
                    Equipo = target,
                    Estado = target.Estado,
                    Detalle = "Equipo actualizado"
                };

                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["DeletedSuccess"], await FetchAll());
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Equipo>>> DeleteTemporal(int id)
        {
            var response = new ServiceResponse<List<Equipo>>();

            try
            {
                // Recuperar equipo
                var equipo = await FetchById(id);

                // Eliminar equipo
                _context.Equipo.Remove(equipo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["DeletedSuccess"], await FetchAll());
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<string>> GetBarcode(int id)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Recuperar activoBodega
                string activoBodega = await _context.Equipo
                    .Where(e => e.Id == id)
                    .Select(e => e.ActivoBodega)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception(_messages["NotFound"]);

                string barcode = Utils.UniqueIdentifierHelper.GenerateBarcode(activoBodega);

                // Configurar respuesta
                response.SetSuccess(_messages["GenerateBarCodeSuccess"], barcode);
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