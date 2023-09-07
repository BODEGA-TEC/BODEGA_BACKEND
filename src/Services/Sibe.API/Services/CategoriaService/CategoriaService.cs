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
        private readonly DataContext _context;
        private const string ErrorPrefix = "[ERROR] - ";

        public CategoriaService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<Categoria>> Create(Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            // Iniciar transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Agregar la nueva categoría a la base de datos
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await _context.Database.CommitTransactionAsync();

                response.Data = categoria;
                response.Success = true;
                response.Message = "Categoría creada con éxito.";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, puedes deshacer la transacción
                await transaction.RollbackAsync();

                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadAll()
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                var categorias = await _context.Categorias.ToListAsync() 
                    ?? throw new Exception("Equipo no encontrado.");

                response.Data = categorias;
                response.Success = true;
                response.Message = categorias.Count == 0
                    ? "No se ha registrado equipo." : "Categorías recuperadas con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Categoria>> ReadById(int id)
        {
            var response = new ServiceResponse<Categoria>();

            try
            {
                var categoria = await _context.Categorias.FindAsync(id)
                    ?? throw new Exception("Categoria no encontrada.");

                response.Data = categoria;
                response.Success = true;
                response.Message = "Categoría obtenida con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Categoria>>> ReadByTipoCategoria(TipoCategoria tipo)
        {
            var response = new ServiceResponse<List<Categoria>>();

            try
            {
                var categorias = await _context.Categorias
                    .Where(c => c.Tipo == tipo)
                    .ToListAsync()
                    ?? throw new Exception("Categoria no encontrada.");

                response.Data = categorias;
                response.Success = true;
                response.Message = "Categorías obtenidas con éxito.";
            }
            catch (Exception ex)
            {
                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Categoria>> Update(int id, Categoria categoria)
        {
            var response = new ServiceResponse<Categoria>();

            // Iniciar transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var categoriaExistente = await _context.Categorias.FindAsync(id)
                    ?? throw new Exception("Categoria no encontrada.");

                // Actualizar los datos de la categoría existente
                categoriaExistente.Descripcion = categoria.Descripcion;
                categoriaExistente.Tipo = categoria.Tipo;

                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await _context.Database.CommitTransactionAsync();

                response.Data = categoriaExistente;
                response.Success = true;
                response.Message = "Categoría actualizada con éxito.";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, puedes deshacer la transacción
                await transaction.RollbackAsync();

                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<object>> Delete(int id)
        {
            var response = new ServiceResponse<object>();

            // Iniciar transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var categoriaExistente = await _context.Categorias.FindAsync(id)
                    ?? throw new Exception("Categoria no encontrada.");

                _context.Categorias.Remove(categoriaExistente);
                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await _context.Database.CommitTransactionAsync();

                response.Success = true;
                response.Message = "Categoría eliminada con éxito.";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, puedes deshacer la transacción
                await transaction.RollbackAsync();

                response.SetError(ErrorPrefix + ex.InnerException?.Message);
            }

            return response;
        }
    }
}