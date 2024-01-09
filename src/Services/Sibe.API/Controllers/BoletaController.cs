using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models;
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

        [HttpGet("{carne}")]
        public async Task<ActionResult<ServiceResponse<object>>> ReadSolicitanteBoletasPendientes(string carne)
        {
            var response = await _boletaService.ReadSolicitanteBoletasPendientes(carne);
            return Ok(response);
        }

        [HttpPost("registrar/prestamo")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBoletaPrestamo(CreateBoletaDto info)
        {
            var response = await _boletaService.CreateBoletaPrestamo(info);
            return Ok(response);
        }

        [HttpPost("registrar/devolucion/{boletaPrestamoId}")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBoletaDevolucion(int boletaPrestamoId, CreateBoletaDto infoDevolucion)
        {
            var response = await _boletaService.CreateBoletaDevolucion(boletaPrestamoId, infoDevolucion);
            return Ok(response);
        }

        [HttpGet("{id}/xml")]
        public async Task<ActionResult<ServiceResponse<string>>> GetBoletaPdf(int id)
        {
            var response = await _boletaService.GetBoletaPdf(id);
            return Ok(response);
        }

        [HttpGet("{id}/enviar")]
        public async Task<ActionResult<ServiceResponse<string>>> SendBoletaByEmail(int id)
        {
            var response = await _boletaService.SendBoletaByEmail(id);
            return Ok(response);
        }
    }
}
