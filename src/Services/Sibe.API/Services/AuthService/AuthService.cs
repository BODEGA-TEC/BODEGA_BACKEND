using Sibe.API.Data;
using Sibe.API.Models;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models.Historicos;
using Sibe.API.Utils;

namespace Sibe.API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly int MAX_PASSWORD_HISTORY_ENTRIES = 4;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly JwtCredentialProvider _jwtCredentialProvider;

        public AuthService(ILogger<AuthService> logger, IConfiguration configuration, DataContext context, JwtCredentialProvider jwtCredentialProvider)
        {
            _logger = logger;
            _messages = configuration.GetSection("UsuarioService");
            _context = context;
            _jwtCredentialProvider = jwtCredentialProvider;
        }

        private async Task<Usuario> GetUsuarioByCorreo(string correo)
        {
            // Recuperar usuario
            return await _context.Usuario
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Equals(correo)) ?? throw new Exception(_messages["UsuarioNotFound"]);
        }


        private async Task IsCarneInUse(string carne)
        {
            if (await _context.Usuario.AnyAsync(u => u.Carne == carne))
                throw new Exception(_messages["CarneInUse"]);
        }

        // Método para asignar la clave temporal al usuario
        private static void SetClaveTemporal(Usuario usuario) => usuario.ClaveTemporal = UniqueIdentifierHelper.GenerateRandomString(length: 8);


        // Método para obtener la contraseña más reciente y activa
        private HistoricoClave GetCurrentClave(Usuario usuario)
        {
            // Ordenar el historial de claves por fecha de cambio de forma descendente
            var currentClave = usuario.HistoricoClaves.OrderByDescending(h => h.FechaCambio).FirstOrDefault();

            // Verificar si la contraseña es null o si ha vencido
            if (currentClave == null || currentClave.FechaCambio.AddMonths(6) < TimeZoneHelper.Now())
            {
                SetClaveTemporal(usuario);
                throw new Exception(currentClave == null ? _messages["ClaveTemporalUnchanged"] : _messages["ExpiredClave"]);
            }

            return currentClave;
        }


        // Metodo que se encarga de la creacion y almacenado de los tokens de autenticacion.
        private async Task<(string AccessToken, string RefreshToken)> CreateTokens(Usuario usuario)
        {
            var rolesNames = usuario.Roles?.Select(r => r.Nombre).ToList() ?? new List<string>();
            var accessToken = _jwtCredentialProvider.CreateToken(usuario.Correo, usuario.Nombre, rolesNames);
            var (refreshToken, fechaCreacion, fechaExpiracion) = _jwtCredentialProvider.GenerateRefreshToken();

            // Almacenar el refresh token
            var historicoRefreshToken = new HistoricoRefreshToken
            {
                Usuario = usuario,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                FechaCreacion = fechaCreacion,
                FechaExpiracion = fechaExpiracion
            };

            // Almacenar el refreshToken
            await _context.HistoricoRefreshToken.AddAsync(historicoRefreshToken);
            await _context.SaveChangesAsync();

            return (accessToken, refreshToken);
        }


        public async Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Si el carne ya se encuentra registrado
                await IsCarneInUse(usuarioDto.Carne);

                // Recuperar roles
                List<Rol> roles = await _context.Rol
                    .Where(r => usuarioDto.RolesIds.Contains(r.Id))
                    .ToListAsync();

                if (roles.Count != usuarioDto.RolesIds.Count)
                {
                    throw new Exception(_messages["RoleNotFound"]);
                }

                // Crear usuario
                var usuario = new Usuario
                {
                    Carne = usuarioDto.Carne,
                    Nombre = usuarioDto.Nombre.ToUpper(),
                    Correo = usuarioDto.Correo, // Valida formato correo
                    Roles = roles
                };
                SetClaveTemporal(usuario);

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

        public async Task<ServiceResponse<Dictionary<string, object>>> Login(string correo, string clave)
        {
            var response = new ServiceResponse<Dictionary<string, object>>();

            try
            {
                // Recuperar usuario
                var usuario = await GetUsuarioByCorreo(correo);

                // Autenticacion
                var currentClave = GetCurrentClave(usuario);
                if (!JwtCredentialProvider.AuthPasswordHash(clave, currentClave.ClaveHash, currentClave.ClaveSalt))
                {
                    throw new Exception(_messages["InvalidCredentials"]);
                }

                // Crear tokens
                var tokens = await CreateTokens(usuario);

                // Configurar respuesta
                var loginResponse = new Dictionary<string, object>
                {
                    { "id", usuario.Id },
                    { "name", usuario.Nombre },
                    { "roles", usuario.Roles },
                    { "accessToken", tokens.AccessToken },
                    { "refreshToken", tokens.RefreshToken }
                };

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], loginResponse);
                _logger.LogInformation("Inicio de sesión exitoso: {Usuario.Nombre}", usuario.Nombre);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
                _logger.LogError(ex, "Se produjo un error al iniciar sesión con el usuario \"{correo}\"", correo);
            }

            return response;
        }


        public async Task<ServiceResponse<Dictionary<string, string>>> RefreshToken(int usuarioId, RefreshTokenDto refreshTokenDto)
        {
            var response = new ServiceResponse<Dictionary<string, string>>();

            try
            {
                var storedRefreshToken = _context.HistoricoRefreshToken
                    .Include(h => h.Usuario)
                    .FirstOrDefault(h =>
                        h.AccessToken == refreshTokenDto.TokenExpirado &&
                        h.RefreshToken == refreshTokenDto.RefreshToken &&
                        h.Usuario.Id == usuarioId)
                    ?? throw new Exception(_messages["RefreshTokenNotFound"]);

                // Crear tokens
                var tokens = await CreateTokens(storedRefreshToken.Usuario);

                // Configurar respuesta
                var responseData = new Dictionary<string, object>
                {
                    { "accessToken", tokens.AccessToken },
                    { "refreshToken", tokens.RefreshToken }
                };

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], responseData);
                _logger.LogInformation("Se ha actualizado refresh token exitosamente");
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
                _logger.LogError(ex, "Se produjo un error al obtener el refresh token");
            }

            return response;
        }


        public async Task<ServiceResponse<object>> ChangeClave(ChangeClaveDto info)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Obtener las contraseñas del usuario.
                var usuario = await _context.Usuario
                    .Include(u => u.HistoricoClaves)
                    .FirstOrDefaultAsync(u => u.Id == info.UsuarioId) ?? throw new Exception(_messages["UsuarioNotFound"]);

                var claves = usuario.HistoricoClaves.OrderByDescending(h => h.FechaCambio).ToList();

                // Crear HistoricoClave
                var historicoClave = new HistoricoClave { Clave = info.ClaveNueva };

                // Crear clave hash y salt
                JwtCredentialProvider.CreatePasswordHash(info.ClaveNueva, out byte[] claveHash, out byte[] claveSalt);
                historicoClave.ClaveHash = claveHash;
                historicoClave.ClaveSalt = claveSalt;

                // Gestionar la antigüedad de las claves directamente
                if (claves.Count >= MAX_PASSWORD_HISTORY_ENTRIES)
                {
                    // Eliminar la más antigua
                    claves.RemoveAt(claves.Count - 1);
                }

                // Agregar la nueva clave al historial del usuario
                claves.Insert(0, historicoClave);

                // Guardar cambios (modificaciones de la lista)
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["RegisterSuccess"]);
                _logger.LogInformation("Registro exitoso: {Nombre}", usuario.Nombre);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
                _logger.LogError(ex, "Se produjo un error al cambiar la clave");
            }

            return response;
        }
    }
}