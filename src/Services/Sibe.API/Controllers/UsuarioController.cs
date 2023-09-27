
using Sibe.API.Models;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Services.UsuarioService;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Usuario;
using Microsoft.AspNetCore.Authorization;

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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Registra un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="request">Datos del nuevo usuario a ser registrado.</param>
        /// <returns>ActionResult<ServiceResponse<int>> Objeto que encapsula el resultado de la operación, incluyendo el ID del usuario registrado.</returns>
        [HttpPost("register")]
        [Authorize(Roles= "ADMINISTRADOR")]
        public async Task<ActionResult<ServiceResponse<object>>> Register(RegisterUsuarioDto request)
        {
            var response = await _usuarioService.Register(request);
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
            var response = await _usuarioService.Login(request.Carne, request.Clave);
            return response.Success ? Ok(response) : Unauthorized(response);
        }
    }
}