using Sibe.API.Data;
using Sibe.API.Models;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models.Historicos;
using Sibe.API.Utils;
using Microsoft.OpenApi.Extensions;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private static readonly string passwordRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d\\/%()_\\-*&@]{8,16}$";
        private readonly int MAX_PASSWORD_HISTORY_ENTRIES = 4;
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly JwtCredentialProvider _jwtCredentialProvider;

        public AuthService(IConfiguration configuration, DataContext context, JwtCredentialProvider jwtCredentialProvider)
        {
            _messages = configuration.GetSection("UsuarioService");
            _context = context;
            _jwtCredentialProvider = jwtCredentialProvider;
        }



        // Funcion para verificar que la clave es de un formato correcto.
        private void ValidateRegexClave(string clave)
        {
            string errorMessage = @"De 8 a 16 caracteres.
                                    Al menos una letra mayúscula.
                                    Al menos una letra minúscula.
                                    Al menos un número.
                                    Al menos un carácter especial: "" /, %, (, ), _, -, *, &, @.""";

            RegexValidator.ValidateWithRegex(clave, passwordRegex, errorMessage);
        }

        private async Task<Usuario> GetUsuarioByUsername(string username)
        {
            return await _context.Usuario.SingleOrDefaultAsync(x => x.Username == username)
                ?? throw new Exception(_messages["UsuarioNotFound"]);
        }

        private async Task IsUsernameInUse(string username)
        {
            if (await _context.Usuario.AnyAsync(x => x.Username == username))
                throw new Exception(_messages["UsernameInUse"]);
        }

        private async Task IsCorreoInUse(string correo)
        {
            if (await _context.Usuario.AnyAsync(x => x.Correo == correo))
                throw new Exception(_messages["CorreoInUse"]);
        }





        // Método para asignar la clave temporal al usuario
        private static void SetClaveTemporal(Usuario usuario) => usuario.ClaveTemporal = UniqueIdentifierHelper.GenerateRandomString(length: 6);


        // Método para obtener la contraseña más reciente y activa
        private HistoricoClave GetCurrentClave(Usuario usuario)
        {
            // Ordenar el historial de claves por fecha de cambio de forma descendente
            var currentClave = usuario.HistoricoClaves.OrderByDescending(h => h.FechaRegistro).FirstOrDefault();

            // Verificar si la contraseña es null o si ha vencido
            var expirationDate = currentClave?.FechaRegistro.AddMonths(6);
            if (currentClave == null || expirationDate < TimeZoneHelper.Now())
            {
                SetClaveTemporal(usuario);
                throw new Exception(currentClave == null ? _messages["ClaveTemporalUnchanged"] : _messages["ExpiredClave"]);
            }

            return currentClave;
        }


        // Metodo que se encarga de la creacion y almacenado de los tokens de autenticacion.
        private async Task<(string AccessToken, string RefreshToken)> CreateTokens(Usuario usuario)
        {
            var accessToken = _jwtCredentialProvider.CreateToken(usuario.Username, usuario.Correo, usuario.Rol.ToString());
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
                // Validaciones
                await IsUsernameInUse(usuarioDto.Username);
                await IsCorreoInUse(usuarioDto.Correo);
                ValidateRegexClave(usuarioDto.Clave);

                // Crear usuario
                var usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Username = usuarioDto.Username,
                    Correo = usuarioDto.Correo,
                    Rol = usuarioDto.Rol
                };

                // Crear clave hash y salt
                JwtCredentialProvider.CreatePasswordHash(usuarioDto.Clave, out byte[] claveHash, out byte[] claveSalt);
                usuario.HistoricoClaves.Add(new HistoricoClave { ClaveHash = claveHash, ClaveSalt = claveSalt });

                // Almacenar en db
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["RegisterSuccess"]);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<Dictionary<string, object>>> Login(string username, string clave)
        {
            var response = new ServiceResponse<Dictionary<string, object>>();

            try
            {
                // Recuperar usuario
                var usuario = await _context.Usuario
                    .Include(u => u.HistoricoClaves)
                    .FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception(_messages["UsuarioNotFound"]);

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
                    { "nombre", usuario.Nombre },
                    { "rol", usuario.Rol },
                    { "accessToken", tokens.AccessToken },
                    { "refreshToken", tokens.RefreshToken }
                };

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], loginResponse);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
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
                var responseData = new Dictionary<string, string>
                {
                    { "accessToken", tokens.AccessToken },
                    { "refreshToken", tokens.RefreshToken }
                };

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], responseData);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<string>> ForgotClave(string correo)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Obtener al usuario con ese correo
                var usuario = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Correo == correo) ?? throw new Exception(_messages["UsuarioNotFound"]);

                // Asignar clave temporal
                SetClaveTemporal(usuario);

                /* Enviar la clave temporal por correo. */

                // Actualizar
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["ClaveTemporalSentSuccess"], usuario.ClaveTemporal);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            return response;
        }



        public async Task<ServiceResponse<object>> ChangeClave(ChangeClaveDto info)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Validar formato de la contraseña
                ValidateRegexClave(info.ClaveNueva);

                // Obtener las contraseñas del usuario.
                var usuario = await _context.Usuario
                    .Include(u => u.HistoricoClaves)
                    .FirstOrDefaultAsync(u => u.Id == info.UsuarioId && u.ClaveTemporal == info.ClaveTemporal) ?? throw new Exception(_messages["ClaveTemporalInvalid"]);
                //var usuario.HistoricoClaves = usuario.HistoricoClaves.OrderByDescending(h => h.FechaRegistro).ToList();  // De las más nueva a la más vieja.

                // Verificar que la clave no esté repetida
                bool duplicatedClave = usuario.HistoricoClaves.Any(currentClave => JwtCredentialProvider.AuthPasswordHash(info.ClaveNueva, currentClave.ClaveHash, currentClave.ClaveSalt));
                if (duplicatedClave) throw new Exception(_messages["ClaveDuplicated"]);

                // Si existen 4 claves almacenadas, se desecha la más vieja.
                if (usuario.HistoricoClaves.Count == MAX_PASSWORD_HISTORY_ENTRIES)
                {
                    // Obtener la clave más antigua
                    var oldestClave = usuario.HistoricoClaves.OrderBy(h => h.FechaRegistro).First();

                    // Eliminar la más antigua de la base de datos
                    _context.HistoricoClave.Remove(oldestClave);

                    // Eliminar la más antigua de la lista en memoria
                    usuario.HistoricoClaves.Remove(oldestClave);
                }

                // Agregar la nueva clave al historial del usuario y despejar la clave temporal
                JwtCredentialProvider.CreatePasswordHash(info.ClaveNueva, out byte[] claveHash, out byte[] claveSalt);
                usuario.HistoricoClaves.Add(new HistoricoClave { ClaveHash = claveHash, ClaveSalt = claveSalt });
                usuario.ClaveTemporal = null;

                // Guardar cambios (modificaciones de la lista)
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["ClaveChangeSuccess"]);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            return response;
        }
    }
}