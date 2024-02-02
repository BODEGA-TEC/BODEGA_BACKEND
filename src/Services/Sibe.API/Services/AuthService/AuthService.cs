using Sibe.API.Data;
using Sibe.API.Models;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models.Historicos;
using Sibe.API.Utils;
using System.Security.Claims;
using Sibe.API.Models.Enums;

namespace Sibe.API.Services.AuthService
{

    /// <summary>
    /// Clase que proporciona servicios de autenticación.
    /// </summary>
    public class AuthService : IAuthService
    {
        private static readonly string passwordRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d\\/%()_\\-*&@]{8,20}$";
        private readonly int MAX_PASSWORD_HISTORY_ENTRIES = 4;
        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly JwtCredentialProvider _jwtCredentialProvider;

        /// <summary>
        /// Inicializa una nueva instancia de la clase AuthService.
        /// </summary>
        /// <param name="configuration">La configuración de la aplicación.</param>
        /// <param name="context">El contexto de datos de la aplicación.</param>
        /// <param name="jwtCredentialProvider">El proveedor de credenciales JWT.</param>
        public AuthService(IConfiguration configuration, DataContext context, JwtCredentialProvider jwtCredentialProvider)
        {
            _messages = configuration.GetSection("UsuarioService");
            _context = context;
            _jwtCredentialProvider = jwtCredentialProvider;
        }
        

        /// <summary>
        /// Función para verificar que la clave tiene un formato correcto.
        /// </summary>
        /// <param name="clave">La clave que se va a verificar.</param>
        private static void ValidateRegexClave(string clave)
        {
            string errorMessage = @"De 8 a 20 caracteres. Al menos una letra mayúscula. Al menos una letra minúscula. Al menos un número. Al menos un carácter especial: "" /, %, (, ), _, -, *, &, @.""";

            RegexValidator.ValidateWithRegex(clave, passwordRegex, errorMessage);
        }


        /// <summary>
        /// Obtiene un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario a buscar.</param>
        /// <returns>El objeto Usuario encontrado.</returns>
        private async Task<Usuario> GetUsuarioByUsername(string username)
        {
            return await _context.Usuario.SingleOrDefaultAsync(x => x.Username == username)
                ?? throw new Exception(_messages["UsuarioNotFound"]);
        }


        /// <summary>
        /// Verifica si el nombre de usuario ya está en uso.
        /// </summary>
        /// <param name="username">El nombre de usuario que se va a verificar.</param>
        private async Task IsUsernameInUse(string username)
        {
            if (await _context.Usuario.AnyAsync(x => x.Username == username))
                throw new Exception(_messages["UsernameInUse"]);
        }


        /// <summary>
        /// Verifica si el correo electrónico ya está en uso.
        /// </summary>
        /// <param name="correo">El correo electrónico que se va a verificar.</param>
        private async Task IsCorreoInUse(string correo)
        {
            if (await _context.Usuario.AnyAsync(x => x.Correo == correo))
                throw new Exception(_messages["CorreoInUse"]);
        }


        /// <summary>
        /// Asigna una clave temporal al usuario.
        /// </summary>
        /// <param name="usuario">El usuario al que se le va a asignar la clave temporal.</param>
        private static void SetClaveTemporal(Usuario usuario) => usuario.ClaveTemporal = UniqueIdentifierHelper.GenerateRandomString(length: 6);


        /// <summary>
        /// Obtiene la contraseña más reciente y activa del usuario.
        /// </summary>
        /// <param name="usuario">El usuario del que se va a obtener la contraseña.</param>
        /// <returns>El objeto HistoricoClave que representa la contraseña actual.</returns>
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


        /// <summary>
        /// Método privado encargado de la creación y almacenamiento de los tokens de autenticación.
        /// </summary>
        /// <param name="usuario">El objeto de usuario para el cual se están generando los tokens.</param>
        /// <returns>Una tupla que contiene el token de acceso y el token de actualización generados.</returns>
        private async Task<(string AccessToken, string RefreshToken)> CreateTokens(Usuario usuario)
        {
            // Crear token de acceso
            var accessToken = _jwtCredentialProvider.CreateToken(usuario.Id, usuario.Username, usuario.Correo, usuario.Rol.ToString());

            // Generar refresh token
            var (refreshToken, fechaCreacion, fechaExpiracion) = _jwtCredentialProvider.GenerateRefreshToken();

            // Almacenar el refresh token en la base de datos
            var historicoRefreshToken = new HistoricoRefreshToken
            {
                Usuario = usuario,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                FechaCreacion = fechaCreacion,
                FechaExpiracion = fechaExpiracion
            };

            // Recuperar  tokens que estén expirados del usuario especificado
            var storedRefreshTokens = _context.HistoricoRefreshToken
                .Where(h =>
                    h.Usuario.Id == usuario.Id &&
                    h.FechaExpiracion < TimeZoneHelper.Now())
                .ToList();

            // Eliminar los refresh token expirados de la base de datos
            _context.HistoricoRefreshToken.RemoveRange(storedRefreshTokens);

            // Almacenar el refresh token en la base de datos
            await _context.HistoricoRefreshToken.AddAsync(historicoRefreshToken);

            // Guardar cambios
            await _context.SaveChangesAsync();

            // Devolver ambos tokens
            return (accessToken, refreshToken);
        }


        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuarioDto">Los datos del usuario que se va a registrar.</param>
        /// <returns>Un objeto de respuesta que indica el resultado del registro.</returns>
        public async Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Validaciones
                await IsUsernameInUse(usuarioDto.Username);
                //await IsCorreoInUse(usuarioDto.Correo);
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

            // Devolver la respuesta
            return response;
        }


        /// <summary>
        /// Realiza el inicio de sesión de un usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario que desea iniciar sesión.</param>
        /// <param name="clave">La contraseña del usuario que desea iniciar sesión.</param>
        /// <returns>Un objeto de respuesta que indica el resultado del inicio de sesión y los tokens de acceso y actualización.</returns>
        public async Task<ServiceResponse<LoginResponse>> Login(string username, string clave)
        {
            var response = new ServiceResponse<LoginResponse>();

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
                var loginResponse = new LoginResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Rol = (int) usuario.Rol,
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                };

                // Configurar respuesta
                response.SetSuccess(_messages["LoginSucess"], loginResponse);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            // Devolver la respuesta y el token de actualización
            return response;
        }
       

        /// <summary>
        /// Refresca el token de acceso de un usuario utilizando un token de actualización válido.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario cuyo token se desea refrescar.</param>
        /// <param name="refreshToken">El token de actualización proporcionado por el usuario.</param>
        /// <returns>Un objeto de respuesta que indica el resultado de la operación y los nuevos tokens generados.</returns>
        public async Task<ServiceResponse<LoginResponse>> RefreshToken(int usuarioId, string refreshToken)
        {
            var response = new ServiceResponse<LoginResponse>();

            try
            {
                // Buscar el refresh token almacenado
                var storedRefreshToken = _context.HistoricoRefreshToken
                    .Include(h => h.Usuario)
                    .FirstOrDefault(h =>
                        h.RefreshToken == refreshToken &&
                        h.Usuario.Id == usuarioId)
                    ?? throw new Exception(_messages["RefreshTokenNotFound"]);

                // Comprobar si el refresh token está expirado
                if (storedRefreshToken.IsExpired)
                {
                    // Lanzar error
                    throw new Exception(_messages["RefreshTokenExpired"]);
                }

                Usuario usuario = storedRefreshToken.Usuario;

                // Comprobar si el access token está expirado, si es asi crear otro
                if (JwtCredentialProvider.IsTokenExpired(storedRefreshToken.AccessToken))
                {
                    // Crear nuevo token 
                    string accessToken = _jwtCredentialProvider.CreateToken(usuario.Id, usuario.Username, usuario.Correo, usuario.Rol.ToString());

                    // Actualizar el token y guardar cambios
                    storedRefreshToken.AccessToken = accessToken;
                    await _context.SaveChangesAsync();
                }

                // Configurar respuesta
                var responseData = new LoginResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Rol = (int)usuario.Rol,
                    AccessToken = storedRefreshToken.AccessToken,
                };

                // Configurar respuesta
                response.SetSuccess(_messages["RefreshSuccess"], responseData);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            // Devolver la respuesta
            return response;
        }


        /// <summary>
        /// Genera y asigna una clave temporal al usuario y envía esta clave temporal por correo electrónico.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario que ha olvidado su contraseña.</param>
        /// <returns>Un objeto de respuesta que indica el resultado de la operación y la clave temporal enviada.</returns>
        public async Task<ServiceResponse<string>> ForgotClave(string username)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Obtener al usuario con ese correo
                var usuario = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception(_messages["UsuarioNotFound"]);

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

            // Devolver la respuesta
            return response;
        }


        /// <summary>
        /// Cambia la contraseña del usuario y actualiza su historial de contraseñas.
        /// </summary>
        /// <param name="info">Información necesaria para cambiar la contraseña del usuario.</param>
        /// <returns>Un objeto de respuesta que indica el resultado de la operación.</returns>
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

            // Devolver la respuesta
            return response;
        }

        public async Task<ServiceResponse<object>> Logout(int usuarioId, string refreshToken, string accessToken)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Buscar la entrada almacenada coincidente
                var storedRefreshToken = _context.HistoricoRefreshToken
                    .Include(h => h.Usuario)
                    .FirstOrDefault(h =>
                        h.AccessToken == accessToken &&
                        h.RefreshToken == refreshToken &&
                        h.Usuario.Id == usuarioId)
                    ?? throw new Exception(_messages["RefreshTokenNotFound"]);

                // Eliminar los refresh token expirados de la base de datos
                _context.HistoricoRefreshToken.RemoveRange(storedRefreshToken);

                // Guardar cambios
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["LogoutSuccess"]);
            }

            catch (Exception ex)
            {
                // Log del error
                response.SetError(ex.Message);
            }

            // Devolver la respuesta
            return response;
        }

    }
}