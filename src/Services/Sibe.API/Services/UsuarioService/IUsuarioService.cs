using Sibe.API.Models.Entidades;
using Sibe.API.Models;
using Sibe.API.Data.Dtos.Usuario;

namespace Sibe.API.Services.UsuarioService
{
    public interface IUsuarioService
    {
        Task<ServiceResponse<object>> Register(RegisterUsuarioDto usuarioDto);
        Task<ServiceResponse<string>> Login(string carne, string clave);
    }
}
