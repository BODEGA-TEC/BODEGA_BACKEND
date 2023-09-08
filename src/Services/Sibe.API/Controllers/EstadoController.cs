using Microsoft.AspNetCore.Mvc;
using Sibe.API.Models;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Controllers
{
    [Route("api/estados")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoService _EstadoService;

        public EstadoController(IEstadoService EstadoService)
        {
            _EstadoService = EstadoService;
        }

        [HttpPost("{desc}")]
        public async Task<ActionResult<ServiceResponse<Estado>>> Create(string desc)
        {
            var response = await _EstadoService.Create(desc);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<Estado>>>> ReadAll()
        {
            var response = await _EstadoService.ReadAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Estado>>> ReadById(int id)
        {
            var response = await _EstadoService.ReadById(id);
            return Ok(response);
        }

        [HttpPut("{id}/{desc}")]
        public async Task<ActionResult<ServiceResponse<Estado>>> Update(int id, string desc)
        {
            var response = await _EstadoService.Update(id, desc);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _EstadoService.Delete(id);
            return Ok(response);
        }
    }
}
