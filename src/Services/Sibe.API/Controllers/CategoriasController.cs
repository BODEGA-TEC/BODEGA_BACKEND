using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Models;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.CategoriaService;

namespace Sibe.API.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [EnableCors("AllowLocalhost3000")]
    //[Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Categoria>>> Create([FromBody] Categoria Categoria)
        {
            var response = await _categoriaService.Create(Categoria);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<Categoria>>>> ReadAll()
        {
            var response = await _categoriaService.ReadAll();
            return Ok(response);
        }

        [HttpGet("equipo")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoCategoriaEquipo()
        {
            var response = await _categoriaService.ReadByTipoCategoria(TipoCategoria.EQUIPO);
            return Ok(response);
        }

        [HttpGet("componentes")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> ReadByTipoCategoriaComponentes()
        {
            var response = await _categoriaService.ReadByTipoCategoria(TipoCategoria.COMPONENTE);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Categoria>>> Update(int id, [FromBody] Categoria Categoria)
        {
            var response = await _categoriaService.Update(id, Categoria);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(int id)
        {
            var response = await _categoriaService.Delete(id);
            return Ok(response);
        }
    }
}
