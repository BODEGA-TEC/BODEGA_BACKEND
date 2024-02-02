
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Usuario;
using Sibe.API.Models;
using Sibe.API.Services.AuthService;

namespace Sibe.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar la autenticación y registro de usuarios.
    /// </summary>
    /// <remarks>
    /// Este controlador proporciona métodos para el registro de usuarios y el inicio de sesión.
    /// </remarks>
    [ApiController]
    [Route("api/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registra un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="request">Datos del nuevo usuario a ser registrado.</param>
        /// <returns>ActionResult<ServiceResponse<int>> Objeto que encapsula el resultado de la operación, incluyendo el ID del usuario registrado.</returns>
        [HttpPost("register")]
        //[Authorize(Roles= "ADMINISTRADOR")]
        public async Task<ActionResult<ServiceResponse<object>>> Register(RegisterUsuarioDto request)
        {
            var response = await _authService.Register(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Autentica a un usuario y devuelve el access y refresh token.
        /// </summary>
        /// <param name="request">Datos de autenticacion.</param>
        /// <returns>ActionResult<ServiceResponse<string>> Object encapsulating the operation result, including the ID of the authenticated user and an access token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginRequest request)
        {
            var response = await _authService.Login(request.Username, request.Clave);
            if (response.Success)
            {
                if (response.Data == null) throw new Exception("Fallo en el login");

                // Establecer la cookie del ID
                Response.Cookies.Append(
                    "user_id",
                     response.Data.Id.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(1), // Expira en 1 día
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    }
                );

                // Establecer la cookie del refresh token
                Response.Cookies.Append(
                    "refresh_token",
                    response.Data.RefreshToken,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(1), // Expira en 1 día
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    }
                );

                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Refresca el token de acceso para un usuario autenticado.
        /// </summary>
        /// <param name="usuarioId">ID del usuario para el cual se va a refrescar el token.</param>
        /// <param name="requestTokenDto">Datos necesarios para el refresco del token.</param>
        /// <returns>ActionResult<ServiceResponse<Dictionary<string, string>>> Objeto que encapsula el resultado de la operación, incluyendo el nuevo token de acceso.</returns>
        [HttpGet("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken()
        {
            var response = new ServiceResponse<LoginResponse>();

            try
            {
                // Obtener cookies
                string usuarioId = Request.Cookies["user_id"] ?? throw new Exception("Id del usuario no proporcionado en las cookies.");
                string refreshToken = Request.Cookies["refresh_token"] ?? throw new Exception("Refresh token no proporcionado en las cookies.");

                // Llamar al servicio de autenticación para refrescar el token
                response = await _authService.RefreshToken(int.Parse(usuarioId), refreshToken);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                response.SetError(ex.Message);
            }

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ServiceResponse<object>>> Logout(LogoutRequest logoutRequest)
        {
            var response = new ServiceResponse<object>();

            try
            {
                // Obtener cookies
                string usuarioId = Request.Cookies["user_id"] ?? throw new Exception("Id del usuario no proporcionado en las cookies.");
                string refreshToken = Request.Cookies["refresh_token"] ?? throw new Exception("Refresh token no proporcionado en las cookies.");

                // Limpiar las cookie en la respuesta
                Response.Cookies.Delete("user_id");
                Response.Cookies.Delete("refresh_token");

                // Llamar al servicio de logout
                response = await _authService.Logout(int.Parse(usuarioId), refreshToken, logoutRequest.AccessToken);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                response.SetError(ex.Message);
            }

            return response.Success ? Ok(response) : BadRequest(response);
        }


        /// <summary>
        /// Setea la clave temporal al usuario propietario del correo y envía este codigo al correo registrado
        /// </summary>
        /// <param name="username">username del usuario para el cual se va generar la clave temporal.</param>
        /// <returns>ActionResult<ServiceResponse<string>> Objeto que encapsula el resultado de la operación.</returns>
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotClave(ForgotClaveDto dto)
        {
            var response = await _authService.ForgotClave(dto.Username);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Cambia la contraseña del usuario autenticado.
        /// </summary>
        /// <param name="info">Datos necesarios para cambiar la contraseña.</param>
        /// <returns>ActionResult<ServiceResponse<object>> Objeto que encapsula el resultado de la operación.</returns>
        [HttpPost("change-password")]
        public async Task<ActionResult<ServiceResponse<object>>> ChangeClave(ChangeClaveDto info)
        {
            var response = await _authService.ChangeClave(info);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}