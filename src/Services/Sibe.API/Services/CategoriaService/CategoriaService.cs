﻿using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {

        // Variables gloables
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;

        public CategoriaService(IConfiguration configuration, DataContext context)
        {
            _context = context;
            _messages = configuration.GetSection("CategoriaService");
        }

        public async Task<ServiceResponse<Categoria>> Create(Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                // Agregar categoría
                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], categoria);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadAll()
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                // Recuperar categorías
                var categorias = await _context.Categoria
                    .ToListAsync()
                    ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                string? message = categorias.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];
                response.SetSuccess(message, categorias);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<Categoria> FetchById(int id)
        {
            // Recuperar categoría
            var categoria = await _context.Categoria
                .FindAsync(id)
                ?? throw new Exception(_messages["NotFound"]);

            return categoria;
        }

        public async Task<Categoria> FetchByNombre(string nombre)
        {
            // Recuperar categoría
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(c => c.Nombre == nombre.ToUpper())
                ?? throw new Exception(_messages["NotFound"]);

            return categoria;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadByTipoCategoria(TipoCategoria tipo)
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                // Recuperar categorías
                var categorias = await _context.Categoria
                    .Where(c => c.Tipo == tipo)
                    .ToListAsync()
                    ?? throw new Exception(_messages["NotFound"]);

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], categorias);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Categoria>> Update(int id, Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                // Recuperar categoría
                var target = await _context.Categoria
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Actualizar categoría
                target.Nombre = categoria.Nombre;
                target.Tipo = categoria.Tipo;
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
                // Recuperar categoría
                var categoria = await _context.Categoria
                    .FindAsync(id)
                    ?? throw new Exception(_messages["NotFound"]);

                // Eliminar categoría
                _context.Categoria.Remove(categoria);
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