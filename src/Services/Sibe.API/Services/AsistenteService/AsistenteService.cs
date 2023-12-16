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


        private async Task IsCarneInUse(string carne)
        {
            if (await _context.Asistente.AnyAsync(a => a.Carne == carne))
                throw new Exception(_messages["CarneInUse"]);
        }
        private async Task IsCorreoInUse(string correo)
        {
            if (await _context.Asistente.AnyAsync(a => a.Correo == correo))
                throw new Exception(_messages["CorreoInUse"]);
        }

        public async Task<Asistente> FetchById(int id)
        {
            // Recuperar asistente
            return await _context.Asistente
                .SingleOrDefaultAsync(a => a.Id == id)
                ?? throw new Exception(_messages["NotFound"]);
        }





        public async Task<ServiceResponse<List<ReadAsistenteDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadAsistenteDto>>();

            try
            {
                // Recuperar asistentes
                var asistentes = await _context.Asistente
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

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
                    .Where(a => a.Activo)
                    .ToListAsync() ?? throw new Exception(_messages["NotFound"]);

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
                var asistente = await _context.Asistente
                    .SingleOrDefaultAsync(a => a.Carne == carne)
                    ?? throw new Exception(_messages["NotFound"]);

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


        public Task<ServiceResponse<object>> RegisterAsistentes(List<RegisterUsuarioDto> asistentesDto)
        {
            throw new NotImplementedException();
        }


        public async Task<ServiceResponse<object>> RegisterHuellaDigitalAsistente(int asistenteId, string data)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Recuperar asistentes
                var asistente = await FetchById(asistenteId);

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
