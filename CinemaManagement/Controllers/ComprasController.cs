using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly CompraService _compraService;

        public ComprasController(CompraService compraService)
        {
            _compraService = compraService;
        }

        // GET: api/Compras
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var compras = await _compraService.ListarTodos();

            return Ok(compras);
        }

        // GET: api/Compras/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var compra = await _compraService.BuscarPorId(id);

            if (compra == null)
                return NotFound("Compra não encontrada.");

            return Ok(compra);
        }

        // POST: api/Compras
        [HttpPost]
        public async Task<IActionResult> Cadastrar(CompraCreateDto dto)
        {
            var resultado = await _compraService.Cadastrar(dto);

            if (resultado.erro != null)
                return BadRequest(resultado.erro);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = resultado.compra!.Id },
                resultado.compra
            );
        }

        // PUT: api/Compras/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            CompraUpdateDto dto)
        {
            var resultado = await _compraService.Atualizar(id, dto);

            if (resultado.erro != null)
                return BadRequest(resultado.erro);

            return Ok(resultado.compra);
        }

        // DELETE: api/Compras/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var sucesso = await _compraService.Excluir(id);

            if (!sucesso)
                return NotFound("Compra não encontrada.");

            return NoContent();
        }
    }
}