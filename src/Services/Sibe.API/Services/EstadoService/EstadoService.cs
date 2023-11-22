using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.EstadoService
{
    public class EstadoService : IEstadoService
    {
        // Variables gloables
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;

        public EstadoService(IConfiguration configuration, DataContext context)
        {
            _context = context;
            _messages = configuration.GetSection("EstadoService");
        }

        public async Task<ServiceResponse<Estado>> Create(string descripcion)
        {
            var response = new ServiceResponse<Estado>();

            try
            {
                // Crear estado
                var estado = new Estado { Nombre = descripcion };

                // Agregar estado
                _context.Estado.Add(estado);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], estado);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        //public async Task<ServiceResponse<List<Estado>>> ReadAll()
        //{
        //    var response = new ServiceResponse<List<Estado>>();

        //    try
        //    {
        //        // Recuperar estados
        //        var estados = await _context.Estado
        //            .ToListAsync()
        //            ?? throw new Exception(_messages["NotFound"]);

        //        // Configurar respuesta
        //        string? message = estados.Count == 0
        //            ? _messages["Empty"]
        //            : _messages["ReadSuccess"];
        //        response.SetSuccess(message, estados);
        //    }

        //    catch (Exception ex)
        //    {
        //        // Configurar error
        //        response.SetError(ex.Message);
        //    }

        //    return response;
        //}

        public async Task<ServiceResponse<List<Estado>>> ReadByTipoActivo(TipoActivo tipo)
        {
            var response = new ServiceResponse<List<Estado>>();

            try
            {
                // Recuperar estados
                var estados = await _context.Estado.ToListAsync() ?? throw new Exception(_messages["NotFound"]);

                // Filtrar estados según el tipo de activo
                if (tipo == TipoActivo.EQUIPO)
                {
                    estados = estados.Where(e => new[]
                        {"DISPONIBLE", "DAÑADO", "PRESTADO", "APARTADO", "EN REPARACION", "RETIRADO"}
                        .Contains(e.Nombre.ToUpper()))
                        .ToList();
                }
                else if (tipo == TipoActivo.COMPONENTE)
                {
                    estados = estados.Where(e => new[] { "DISPONIBLE", "DAÑADO", "AGOTADO" }
                        .Contains(e.Nombre.ToUpper()))
                        .ToList();
                }

                // Configurar respuesta
                string? message = estados.Count == 0 ? _messages["Empty"] : _messages["ReadSuccess"];
                response.SetSuccess(message, estados);
            }
            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<Estado> FetchById(int id)
        {
            // Recuperar estado
            var estado = await _context.Estado
                .FindAsync(id)
                ?? throw new Exception(_messages["NotFound"]);

            return estado;
        }

        public async Task<Estado> FetchByNombre(string nombre)
        {
            // Recuperar estado
            var estado = await _context.Estado
                .FirstOrDefaultAsync(e => e.Nombre == nombre.ToUpper())
                ?? throw new Exception(_messages["NotFound"]);

            return estado;
        }

        public async Task<ServiceResponse<Estado>> Update(int id, string descripcion)
        {
            var response = new ServiceResponse<Estado>();

            try
            {
                // Recuperar estado
                var target = await _context.Estado
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Actualizar estado
                target.Nombre = descripcion;
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["UpdatedSuccess"], target);
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
                // Recuperar estado
                var estadoExistente = await _context.Estado
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Eliminar estado
                _context.Estado.Remove(estadoExistente);
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