using Sibe.API.Models;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models.Enums;

namespace Sibe.API.Services.BoletaService
{
    public interface IBoletaService
    {
        Task<ServiceResponse<List<string>>> ReadAll();
        Task<ServiceResponse<List<string>>> ReadByDateRange(DateTime inicial, DateTime final);
        Task<ServiceResponse<int>> CreateBoletaPrestamo(CreateBoletaDto info);
        Task<ServiceResponse<int>> CreateBoletaDevolucion(CreateBoletaDto info);
        Task<ServiceResponse<string>> CreateBoletaPrestamoXMLToBase64(CreateBoletaDto info);
    }
}
