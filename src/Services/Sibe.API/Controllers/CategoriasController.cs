using Microsoft.AspNetCore.Mvc;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Services.CategoriaService;

namespace Sibe.API.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _CategoriaService;

        public CategoriaController(ICategoriaService CategoriaService)
        {
            _CategoriaService = CategoriaService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Categoria>>> Create([FromBody] Categoria Categoria)
        {
            var response = await _CategoriaService.Create(Categoria);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<Categoria>>>> ReadAll()
        {
            var response = await _CategoriaService.ReadAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadById(int id)
        {
            var response = await _CategoriaService.ReadById(id);
            return Ok(response);
        }

        [HttpGet("equipo")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoCategoriaEquipo()
        {
            var response = await _CategoriaService.ReadByTipoCategoria(TipoCategoria.EQUIPO);
            return Ok(response);
        }

        [HttpGet("componentes")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoCategoriaComponentes()
        {
            var response = await _CategoriaService.ReadByTipoCategoria(TipoCategoria.COMPONENTE);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> Update(int id, [FromBody] Categoria Categoria)
        {
            var response = await _CategoriaService.Update(id, Categoria);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _CategoriaService.Delete(id);
            return Ok(response);
        }
    }
}
