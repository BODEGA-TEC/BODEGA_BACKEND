using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Controllers
{
    [ApiController]
    [Route("api/estados")]
    //[Authorize]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoService _estadoService;

        public EstadoController(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Estado>>> Create([FromBody] Estado estado)
        {
            // No se utiliza estado.Id en esta acción.
            var response = await _estadoService.Create(estado.Nombre);
            return Ok(response);
        }

        //[HttpGet("")]
        //public async Task<ActionResult<ServiceResponse<List<Estado>>>> ReadAll()
        //{
        //    var response = await _estadoService.ReadAll();
        //    return Ok(response);
        //}

        [HttpGet("equipo")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoActivoEquipo()
        {
            var response = await _estadoService.ReadByTipoActivo(TipoActivo.EQUIPO);
            return Ok(response);
        }

        [HttpGet("componentes")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoActivoComponentes()
        {
            var response = await _estadoService.ReadByTipoActivo(TipoActivo.COMPONENTE);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Estado>>> Update(int id, [FromBody] Estado estado)
        {
            var response = await _estadoService.Update(id, estado.Nombre);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _estadoService.Delete(id);
            return Ok(response);
        }
    }
}
