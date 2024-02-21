using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.EquipoService;

namespace Sibe.API.Controllers
{
    [ApiController]
    [Route("api/equipo")]
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoService _equipoService;

        public EquipoController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<ServiceResponse<ReadEquipoDto>>> Create([FromBody] CreateEquipoDto equipo)
        {
            var response = await _equipoService.Create(equipo);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<ReadEquipoDto>>>> ReadAll()
        {
            var response = await _equipoService.ReadAll();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Equipo>>> ReadById(int id)
        {
            var response = await _equipoService.ReadById(id);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<ReadEquipoDto>>> Update(int id, [FromBody] UpdateEquipoDto equipo)
        {
            var response = await _equipoService.Update(id, equipo);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _equipoService.Delete(id);
            return response.Success ? Ok(response) : BadRequest(response);

        }

    }
}
