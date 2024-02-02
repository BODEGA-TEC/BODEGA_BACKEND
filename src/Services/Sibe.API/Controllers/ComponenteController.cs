using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.ComponenteService;

namespace Sibe.API.Controllers
{
    
    [ApiController]
    [Route("api/componentes")]
    public class ComponenteController : ControllerBase
    {
        private readonly IComponenteService _componenteService;

        public ComponenteController(IComponenteService componenteService)
        {
            _componenteService = componenteService;
        }

        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<ServiceResponse<Componente>>> Create([FromBody] CreateComponenteDto componente)
        {
            var response = await _componenteService.Create(componente);
            return Ok(response);
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<Componente>>>> ReadAll()
        {
            var response = await _componenteService.ReadAll();
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Componente>>> ReadById(int id)
        {
            var response = await _componenteService.ReadById(id);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Componente>>> Update(int id, [FromBody] UpdateComponenteDto componente)
        {
            var response = await _componenteService.Update(id, componente);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _componenteService.Delete(id);
            return Ok(response);
        }
    }
}
