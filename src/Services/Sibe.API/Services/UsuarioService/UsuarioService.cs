using Sibe.API.Data;
using Sibe.API.Models;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Usuario;

namespace Sibe.API.Services.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly JwtCredentialProvider _jwtCredentialProvider;

        public UsuarioService(ILogger<UsuarioService> logger, IConfiguration configuration, DataContext context, JwtCredentialProvider jwtCredentialProvider)
        {
            _logger = logger;
            _messages = configuration.GetSection("UsuarioService");
            _context = context;
            _jwtCredentialProvider = jwtCredentialProvider;
        }

        private async Task IsCarneInUse(string carne)
        {
            if (await _context.Usuario.AnyAsync(u => u.Carne == carne))
                throw new Exception(_messages["CarneInUse"]);
        }

        public async Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Si el carne ya se encuentra registrado
                await IsCarneInUse(usuarioDto.Carne);

                // Recuperar rol
                var rol = await _context.Rol.FindAsync(usuarioDto.RolId)
                    ?? throw new Exception(_messages["RoleNotFound"]);

                // Crear hash
                JwtCredentialProvider.CreatePasswordHash(usuarioDto.Clave, out byte[] passwordHash, out byte[] passwordSalt);

                var usuario = new Usuario
                {
                    Carne = usuarioDto.Carne,
                    Nombre = usuarioDto.Nombre,
                    ClaveHash = passwordHash,
                    ClaveSalt = passwordSalt,
                    Correo = usuarioDto.Correo,
                    Rol = rol
                };

                // Agregar usuario
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["RegisterSuccess"]);
                _logger.LogInformation("Registro exitoso: {Nombre}", usuario.Nombre);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
                _logger.LogError(ex, "Se produjo un error al registrar al usuario {Nombre}", usuarioDto.Nombre);
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Login(string carne, string clave)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Recuperar usuario
                var usuario = await _context.Usuario
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Carne.Equals(carne));

                // Autenticacion
                if (usuario == null || !JwtCredentialProvider.AuthPasswordHash(clave, usuario.ClaveHash, usuario.ClaveSalt))
                {
                    throw new Exception(_messages["InvalidCredentials"]);
                }

                // Crear token
                var token = _jwtCredentialProvider.CreateToken(usuario.Carne, usuario.Nombre, usuario.Rol.Nombre);

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], token);
                _logger.LogInformation("Inicio de sesión exitoso: {Usuario.Nombre}", usuario.Nombre);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
                _logger.LogError(ex, "Se produjo un error al iniciar sesión con el usuario {carne}", carne);
            }

            return response;
        }

    }
}