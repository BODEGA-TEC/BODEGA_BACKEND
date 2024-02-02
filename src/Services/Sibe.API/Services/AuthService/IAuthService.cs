using Sibe.API.Models;
using Sibe.API.Data.Dtos.Usuario;

namespace Sibe.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto);
        Task<ServiceResponse<LoginResponse>> Login(string carne, string clave);
        Task<ServiceResponse<Dictionary<string, string>>> RefreshToken(int usuarioId, string RefreshToken);
        Task<ServiceResponse<string>> ForgotClave(string username);
        Task<ServiceResponse<object>> ChangeClave(ChangeClaveDto info);
        Task<ServiceResponse<object>> Logout(int usuarioId, string refreshToken, string accessToken);
    }
}
