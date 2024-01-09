using Sibe.API.Models;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Entidades;

namespace Sibe.API.Services.BoletaService
{
    public interface IBoletaService
    {
        Task<ServiceResponse<List<ReadBoletaDto>>> ReadAll();
        Task<ServiceResponse<List<ReadBoletaDto>>> ReadByDateRange(DateTime inicial, DateTime final);
        Task<ServiceResponse<object>> ReadSolicitanteBoletasPendientes(string carne);
        Solicitante AuthenticateSolicitante(string carne);
        Task<ServiceResponse<int>> CreateBoletaPrestamo(CreateBoletaDto info);
        Task<ServiceResponse<int>> CreateBoletaDevolucion(int boletaPrestamoId, CreateBoletaDto infoDevolucion);
        Task<ServiceResponse<string>> GetBoletaPdf(int boletaId);
        Task<ServiceResponse<string>> SendBoletaByEmail(int boletaId);
    }
}