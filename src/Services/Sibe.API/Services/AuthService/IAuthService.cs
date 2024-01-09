using Sibe.API.Models;
using Sibe.API.Data.Dtos.Usuario;

namespace Sibe.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto);
        Task<ServiceResponse<Dictionary<string, object>>> Login(string carne, string clave);
        Task<ServiceResponse<Dictionary<string, string>>> RefreshToken(int usuarioId, string RefreshToken, string TokenExpirado);
        Task<ServiceResponse<string>> ForgotClave(string username);
        Task<ServiceResponse<object>> ChangeClave(ChangeClaveDto info);
    }
}
