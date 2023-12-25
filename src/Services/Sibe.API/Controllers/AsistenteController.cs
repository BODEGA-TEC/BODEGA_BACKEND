using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Services.AsistenteService;
using Sibe.API.Services.EquipoService;

namespace Sibe.API.Controllers
{

    [ApiController]
    [Route("api/asistentes")]
    public class AsistenteController : ControllerBase
    {
        private readonly IAsistenteService _asistenteService;

        public AsistenteController(IAsistenteService asistenteService)
        {
            _asistenteService = asistenteService;
        }


        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<ReadAsistenteDto>>>> ReadAll()
        {
            var response = await _asistenteService.ReadAll();
            return Ok(response);
        }

        [HttpGet("activos")]
        public async Task<ActionResult<ServiceResponse<List<ReadAsistenteDto>>>> ReadAllActivo()
        {
            var response = await _asistenteService.ReadAllActivo();
            return Ok(response);
        }

        [HttpGet("{carne}")]
        public async Task<ActionResult<ServiceResponse<List<ReadAsistenteDto>>>> ReadByCarne(string carne)
        {
            var response = await _asistenteService.ReadByCarne(carne);
            return Ok(response);
        }

        [HttpGet("auth")]
        public async Task<ActionResult<ServiceResponse<ReadAsistenteDto>>> ReadByHuellaDigital([FromBody] string huellaDigital)
        {
            var response = await _asistenteService.ReadByHuellaDigital(huellaDigital);
            return Ok(response);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<ServiceResponse<object>>> RegisterAsistentes(List<RegisterAsistenteDto> asistentesDto)
        {
            var response = await _asistenteService.RegisterAsistentes(asistentesDto);
            return Ok(response);
        }

        [HttpPost("registrar/{id}/huella")]
        public async Task<ActionResult<ServiceResponse<object>>> RegisterHuellaDigitalAsistente(string carne, [FromBody] string huellaDigital)
        {
            var response = await _asistenteService.RegisterHuellaDigitalAsistente(carne, huellaDigital);
            return Ok(response);
        }

    }
}
