using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Models;

namespace Sibe.API.Services.EstadoService
{
    public class EstadoService : IEstadoService
    {
        // Variables gloables
        private readonly DataContext _context;
        private readonly EstadoServiceMessages _message;

        // Clase interna para gestionar los mensajes
        private class EstadoServiceMessages
        {
            public readonly string NotFound = "Estado no encontrado.";
            public readonly string CreateSuccess = "Estado creado con éxito.";
            public readonly string ReadSuccess = "Estado(s) recuperado(s) con éxito.";
            public readonly string Empty = "No se han registrado estados.";
            public readonly string UpdatedSuccess = "Estado actualizado con éxito.";
            public readonly string DeletedSuccess = "Estado eliminado con éxito.";
        }

        public EstadoService(DataContext context)
        {
            _context = context;
            _message = new EstadoServiceMessages();
        }

        public async Task<ServiceResponse<Estado>> Create(string descripcion)
        {
            var response = new ServiceResponse<Estado>();

            try
            {
                // Crear estado
                var estado = new Estado { Descripcion = descripcion };

                // Agregar estado
                _context.Estado.Add(estado);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.Data = estado;
                response.SetSuccess(_message.CreateSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Estado>>> ReadAll()
        {
            var response = new ServiceResponse<List<Estado>>();

            try
            {
                // Recuperar estados
                var estados = await _context.Estado
                    .ToListAsync()
                    ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = estados;
                string message = estados.Count == 0
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

        public async Task<ServiceResponse<Estado>> ReadById(int id)
        {
            var response = new ServiceResponse<Estado>();

            try
            {
                // Recuperar estado
                var estado = await _context.Estado
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Configurar respuesta
                response.Data = estado;
                response.SetSuccess(_message.ReadSuccess);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Estado>> Update(int id, string descripcion)
        {
            var response = new ServiceResponse<Estado>();

            try
            {
                // Recuperar estado
                var target = await _context.Estado
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Actualizar estado
                target.Descripcion = descripcion;
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.Data = target;
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
                // Recuperar estado
                var estadoExistente = await _context.Estado
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Eliminar estado
                _context.Estado.Remove(estadoExistente);
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