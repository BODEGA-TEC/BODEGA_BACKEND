using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Services.BoletaService;

namespace Sibe.API.Controllers
{

    [ApiController]
    [Route("api/boletas")]
    public class BoletaController : ControllerBase
    {
        private readonly IBoletaService _boletaService;

        public BoletaController(IBoletaService boletaService)
        {
            _boletaService = boletaService;
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> ReadAll()
        {
            var response = await _boletaService.ReadAll();
            return Ok(response);
        }

        [HttpGet("fecha/{inicial}/{final}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> ReadByDateRange(DateTime inicial, DateTime final)
        {
            var response = await _boletaService.ReadByDateRange(inicial, final);
            return Ok(response);
        }

        [HttpPost("registrar/prestamo")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBoletaPrestamo(CreateBoletaDto info)
        {
            var response = await _boletaService.CreateBoletaPrestamo(info);
            return Ok(response);
        }

        [HttpPost("registrar/devolucion")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBoletaDevolucion(CreateBoletaDto info)
        {
            var response = await _boletaService.CreateBoletaDevolucion(info);
            return Ok(response);
        }

        //[HttpPost("prestamo/base64")]
        //public async Task<ActionResult<ServiceResponse<string>>> CreateBoletaPrestamoXMLToBase64(CreateBoletaDto info)
        //{
        //    var response = await _boletaService.CreateBoletaPrestamoXMLToBase64(info);
        //    return Ok(response);
        //}
    }
}
