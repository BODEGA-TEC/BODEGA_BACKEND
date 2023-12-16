
using Sibe.API.Models;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Services.AuthService;
using Sibe.API.Data.Dtos.Usuario;

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
        /// Authenticates a user and returns an access token.
        /// </summary>
        /// <param name="request">User data for authentication.</param>
        /// <returns>ActionResult<ServiceResponse<string>> Object encapsulating the operation result, including the ID of the authenticated user and an access token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUsuarioDto request)
        {
            var response = await _authService.Login(request.Correo, request.Clave);
            return response.Success ? Ok(response) : BadRequest(response);
        }


        /// <summary>
        /// Refresca el token de acceso para un usuario autenticado.
        /// </summary>
        /// <param name="usuarioId">ID del usuario para el cual se va a refrescar el token.</param>
        /// <param name="requestTokenDto">Datos necesarios para el refresco del token.</param>
        /// <returns>ActionResult<ServiceResponse<Dictionary<string, string>>> Objeto que encapsula el resultado de la operación, incluyendo el nuevo token de acceso.</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ServiceResponse<Dictionary<string, string>>>> RefreshToken(int usuarioId, RefreshTokenDto requestTokenDto)
        {
            var response = await _authService.RefreshToken(usuarioId, requestTokenDto);
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