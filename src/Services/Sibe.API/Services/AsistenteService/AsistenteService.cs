using AutoMapper;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Inventario;
using Sibe.API.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sibe.API.Services.AsistenteService
{
    public class AsistenteService : IAsistenteService
    {
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public AsistenteService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _messages = configuration.GetSection("AsistenteService");
            _context = context;
            _mapper = mapper;
        }


        //private async Task IsCarneInUse(string carne)
        //{
        //    if (await _context.Asistente.AnyAsync(a => a.Carne == carne))
        //        throw new Exception(_messages["CarneInUse"]);
        //}
        //private async Task IsCorreoInUse(string correo)
        //{
        //    if (await _context.Asistente.AnyAsync(a => a.Correo == correo))
        //        throw new Exception(_messages["CorreoInUse"]);
        //}


        public async Task<Asistente> FetchByCarne(string carne)
        {
            // Recuperar asistente
            return await _context.Asistente
                .SingleOrDefaultAsync(a => a.Carne == carne)
                ?? throw new Exception(_messages["NotFound"]);
        }



        public async Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadAsistenteDto>>();

            try
            {
                // Recuperar asistentes
                var asistentes = await _context.Asistente
                    .ToListAsync()
                    ?? throw new Exception(_messages["NotFound"]);

                // Map a Dto
                List<ReadAsistenteDto> dto = _mapper.Map<List<ReadAsistenteDto>>(asistentes);

                // Configurar respuesta
                string? message = dto.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];

                // Configurar respuesta
                response.SetSuccess(message, dto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAllActivo()
        {
            var response = new ServiceResponse<List<ReadAsistenteDto>>();

            try
            {
                // Recuperar asistentes
                var asistentes = await _context.Asistente
                    .Where(a => a.FechaRegistro >= DateTime.UtcNow.AddMonths(-6))
                    .ToListAsync()
                    ?? throw new Exception(_messages["NotFound"]);

                // Map a Dto
                List<ReadAsistenteDto> dto = _mapper.Map<List<ReadAsistenteDto>>(asistentes);

                // Configurar respuesta
                string? message = dto.Count == 0
                    ? _messages["Empty"]
                    : _messages["ReadSuccess"];

                // Configurar respuesta
                response.SetSuccess(message, dto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<ReadAsistenteDto>> ReadByCarne(string carne)
        {
            var response = new ServiceResponse<ReadAsistenteDto>();

            try
            {
                // Recuperar asistentes
                var asistente = await FetchByCarne(carne);

                // Map a Dto
                ReadAsistenteDto dto = _mapper.Map<ReadAsistenteDto>(asistente);

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], dto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<ReadAsistenteDto>> ReadByHuellaDigital(string data)
        {
            var response = new ServiceResponse<ReadAsistenteDto>();

            try
            {
                // Recuperar asistentes
                var asistente = await _context.Asistente
                    .SingleOrDefaultAsync(a => a.HuellaDigital == data)
                    ?? throw new Exception(_messages["NotFound"]);

                if (!asistente.Activo)
                {
                    throw new Exception(_messages["Inactive"]);
                }


                // Map a Dto
                ReadAsistenteDto dto = _mapper.Map<ReadAsistenteDto>(asistente);

                // Configurar respuesta
                response.SetSuccess(_messages["AuthSuccess"], dto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<object>> RegisterAsistentes(List<RegisterAsistenteDto> asistentesDto)
        {
            var response = new ServiceResponse<object>();

            try
            {
                foreach (var asistenteDto in asistentesDto)
                {
                    // Verificar si ya existe un asistente con el mismo correo y carné
                    var existingAsistente = await _context.Asistente
                        .FirstOrDefaultAsync(a => a.Correo == asistenteDto.Correo && a.Carne == asistenteDto.Carne);

                    if (existingAsistente != null)
                    {
                        // Si existe, actualiza la fecha de registro
                        existingAsistente.FechaRegistro = TimeZoneHelper.Now();
                    }

                    else
                    {
                        // Si no existe, crea un nuevo asistente
                        var newAsistente = new Asistente
                        {
                            Nombre = asistenteDto.Nombre,
                            Carne = asistenteDto.Carne,
                            Correo = asistenteDto.Correo,
                        };

                        // Añadir el nuevo asistente al contexto
                        _context.Asistente.Add(newAsistente);
                    }
                }
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["RegisterSuccess"]);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<object>> RegisterHuellaDigitalAsistente(string carne, string data)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Recuperar asistentes
                var asistente = await FetchByCarne(carne);

                // Guardar huella
                asistente.HuellaDigital = data;
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"]);
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
