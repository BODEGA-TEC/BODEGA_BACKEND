using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Services.AsistenteService;

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

        [Authorize(Roles = "ADMIN")]
        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<ReadAsistenteDto>>>> ReadAll()
        {
            var response = await _asistenteService.ReadAll();
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{carne}")]
        public async Task<ActionResult<ServiceResponse<List<ReadAsistenteDto>>>> ReadByCarne(string carne)
        {
            var response = await _asistenteService.ReadByCarne(carne);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("registrar")]
        public async Task<ActionResult<ServiceResponse<object>>> RegisterAsistentes(List<RegisterAsistenteDto> asistentesDto)
        {
            var response = await _asistenteService.RegisterAsistentes(asistentesDto);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{carne}/inscribir/credenciales")]
        public async Task<ActionResult<ServiceResponse<object>>> RegisterAsistenteCredentials(string carne, [FromBody] RegisterCredentialsDto credentialsDto)
        {
            var response = await _asistenteService.RegisterAsistenteCredentials(carne, credentialsDto.Pin, credentialsDto.HuellaDigitalImagen);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("autenticar/huella")]
        public async Task<ActionResult<ServiceResponse<ReadAsistenteDto>>> AuthenticateWithFingerprint([FromBody] FingerprintDto credentialsDto)
        {
            var response = await _asistenteService.AuthenticateAsistente(credentialsDto.HuellaDigitalImagen);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("autenticar/pin")]
        public async Task<ActionResult<ServiceResponse<ReadAsistenteDto>>> AuthenticateWithPin([FromBody] PinDto credentialsDto)
        {
            var response = await _asistenteService.AuthenticateAsistente(credentialsDto.Carne, credentialsDto.Pin);
            return Ok(response);
        }

    }
}
