using AutoMapper;
using iText.Kernel.Pdf;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly JwtCredentialProvider _jwtCredentialProvider;


        public AsistenteService(IConfiguration configuration, DataContext context, IMapper mapper, JwtCredentialProvider jwtCredentialProvider)
        {
            _messages = configuration.GetSection("AsistenteService");
            _context = context;
            _mapper = mapper;
            _jwtCredentialProvider = jwtCredentialProvider;

        }


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


        //public async Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAllActivo()
        //{
        //    var response = new ServiceResponse<List<ReadAsistenteDto>>();

        //    try
        //    {
        //        // Recuperar asistentes
        //        var asistentes = await _context.Asistente
        //            .Where(a => a.FechaRegistro >= DateTime.UtcNow.AddMonths(-6))
        //            .ToListAsync()
        //            ?? throw new Exception(_messages["NotFound"]);

        //        // Map a Dto
        //        List<ReadAsistenteDto> dto = _mapper.Map<List<ReadAsistenteDto>>(asistentes);

        //        // Configurar respuesta
        //        string? message = dto.Count == 0
        //            ? _messages["Empty"]
        //            : _messages["ReadSuccess"];

        //        // Configurar respuesta
        //        response.SetSuccess(message, dto);
        //    }

        //    catch (Exception ex)
        //    {
        //        // Configurar error
        //        response.SetError(ex.Message);
        //    }

        //    return response;
        //}


        public async Task<ServiceResponse<ReadAsistenteDto>> ReadByCarne(string carne)
        {
            var response = new ServiceResponse<ReadAsistenteDto>();

            try
            {
                // Recuperar asistente
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


        public async Task<ServiceResponse<object>> RegisterAsistentes(List<RegisterAsistenteDto> asistentesDto)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Eliminar todos los asistentes existentes en la base de datos
                _context.Asistente.RemoveRange(_context.Asistente);


                // Crear nuevos asistentes con la lista recibida
                foreach (var asistenteDto in asistentesDto)
                {
                    //// Verificar si ya existe un asistente con el mismo correo y carné
                    //var existingAsistente = await _context.Asistente
                    //    .FirstOrDefaultAsync(a => a.Correo == asistenteDto.Correo && a.Carne == asistenteDto.Carne);

                    //if (existingAsistente != null)
                    //{
                    //    // Si existe, actualiza la fecha de registro
                    //    existingAsistente.FechaRegistro = TimeZoneHelper.Now();
                    //}

                    //else
                    //{
                    // Si no existe, crea un nuevo asistente
                    var newAsistente = new Asistente
                    {
                        Nombre = asistenteDto.Nombre,
                        Carne = asistenteDto.Carne,
                        Correo = asistenteDto.Correo,
                    };

                    // Añadir el nuevo asistente al contexto
                    _context.Asistente.Add(newAsistente);
                    //}
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


        public async Task<ServiceResponse<object>> RegisterAsistenteCredentials(string carne, string pin, string fingerprint)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Recuperar asistentes
                var asistente = await FetchByCarne(carne);

                // Guardar credenciales
                JwtCredentialProvider.CreatePinHash(pin, out byte[] pinHash, out byte[] pinSalt);
                asistente.PinHash = pinHash;
                asistente.PinSalt = pinSalt;
                asistente.Fingerprint = fingerprint;

                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreadentialsRegisterSuccess"]);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<ReadAsistenteDto>> AuthenticateAsistente(string fingerprint)
        {
            var response = new ServiceResponse<ReadAsistenteDto>();

            try
            {
                // Recuperar asistente
                var asistente = await _context.Asistente
                    .SingleOrDefaultAsync(a => a.Fingerprint == fingerprint)
                    ?? throw new Exception(_messages["NotFound"]);

                // Crear y almacenar el token para el asistente
                var token = _jwtCredentialProvider.CreateAsistenteToken(asistente.Carne, 10); // 10 minutos de expiración

                // Map a Dto
                ReadAsistenteDto dto = _mapper.Map<ReadAsistenteDto>(asistente);
                dto.Token = token;

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

        public async Task<ServiceResponse<ReadAsistenteDto>> AuthenticateAsistente(string carne, string pin)
        {
            var response = new ServiceResponse<ReadAsistenteDto>();

            try
            {
                // Recuperar asistentes
                var asistente = await FetchByCarne(carne);

                // Verificar que PinHash y PinSalt no sean nulos
                if (asistente.PinHash == null || asistente.PinSalt == null)
                {
                    throw new Exception(_messages["UnsetPinCredentials"]);
                }

                // Verificar el PIN
                if (!JwtCredentialProvider.AuthPinHash(pin, asistente.PinHash, asistente.PinSalt))
                {
                    throw new Exception(_messages["InvalidCredentials"]);
                }

                // Crear y almacenar el token para el asistente
                var token = _jwtCredentialProvider.CreateAsistenteToken(asistente.Carne, 10); // 10 minutos de expiración

                // Map a Dto
                ReadAsistenteDto dto = _mapper.Map<ReadAsistenteDto>(asistente);
                dto.Token = token;

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
    }
}
