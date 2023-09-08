using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Models;
using Sibe.API.Models.Enums;

namespace Sibe.API.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        // Variables gloables
        private readonly DataContext _context;
        private readonly CategoriaServiceMessages _message;

        // Clase interna para gestionar los mensajes
        private class CategoriaServiceMessages
        {
            public readonly string NotFound = "Categoría no encontrada.";
            public readonly string CreateSuccess = "Categoría creada con éxito.";
            public readonly string ReadSuccess = "Categoría(s) recuperada(s) con éxito.";
            public readonly string Empty = "No se han registrado categorias.";
            public readonly string UpdatedSuccess = "Categoría actualizada con éxito.";
            public readonly string DeletedSuccess = "Categoría eliminada con éxito.";
        }

        public CategoriaService(DataContext context)
        {
            _context = context;
            _message = new CategoriaServiceMessages();
        }

        public async Task<ServiceResponse<Categoria>> Create(Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                // Agregar la categoría
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                // Configurar la respuesta
                response.Data = categoria;
                response.SetSuccess(_message.CreateSuccess);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadAll()
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                // Recuperar las categorías
                var categorias = await _context.Categorias
                    .ToListAsync() 
                    ?? throw new Exception(_message.NotFound);

                // Configurar la respuesta
                response.Data = categorias;
                string message = categorias.Count == 0 
                    ? _message.Empty 
                    : _message.ReadSuccess;
                response.SetSuccess(message);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Categoria>> ReadById(int id)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                // Recuperar la categoría
                var categoria = await _context.Categorias
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Configurar la respuesta
                response.Data = categoria;
                response.SetSuccess(_message.ReadSuccess);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadByTipoCategoria(TipoCategoria tipo)
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                // Recuperar las categorías
                var categorias = await _context.Categorias
                    .Where(c => c.Tipo == tipo)
                    .ToListAsync()
                    ?? throw new Exception(_message.NotFound);

                // Configurar la respuesta
                response.Data = categorias;
                response.SetSuccess(_message.ReadSuccess);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Categoria>> Update(int id, Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                // Recuperar la categoría
                var target = await _context.Categorias
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Actualizar la categoría
                target.Descripcion = categoria.Descripcion;
                target.Tipo = categoria.Tipo;
                await _context.SaveChangesAsync();

                // Configurar la respuesta
                response.Data = target;
                response.SetSuccess(_message.UpdatedSuccess);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<object>> Delete(int id)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Recuperar la categoría
                var categoriaExistente = await _context.Categorias
                    .FindAsync(id)
                    ?? throw new Exception(_message.NotFound);

                // Eliminar la categoría
                _context.Categorias.Remove(categoriaExistente);
                await _context.SaveChangesAsync();

                // Configurar la respuesta
                response.SetSuccess(_message.DeletedSuccess);
            }

            catch (Exception ex)
            {
                // Configurar el error
                response.SetError(ex.Message);
            }

            return response;
        }
    }
}