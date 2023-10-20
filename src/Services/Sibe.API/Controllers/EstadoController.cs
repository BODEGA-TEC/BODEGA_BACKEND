﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sibe.API.Models;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.EstadoService;

namespace Sibe.API.Controllers
{
    [ApiController]
    [Route("api/estados")]
    //[Authorize]
    [EnableCors("AllowLocalhost3000")]
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

        [HttpGet("")]
        public async Task<ActionResult<ServiceResponse<List<Estado>>>> ReadAll()
        {
            var response = await _estadoService.ReadAll();
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
