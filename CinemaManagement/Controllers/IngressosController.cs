using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngressosController : ControllerBase
    {
        private readonly IngressoService _ingressoService;

        public IngressosController(IngressoService ingressoService)
        {
            _ingressoService = ingressoService;
        }

        // GET: api/ingressos
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var ingressos = await _ingressoService.ListarTodos();

            return Ok(ingressos);
        }

        // GET: api/ingressos/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var ingresso = await _ingressoService.BuscarPorId(id);

            if (ingresso == null)
                return NotFound(new { mensagem = "Ingresso não encontrado." });

            return Ok(ingresso);
        }

        // POST: api/ingressos
        [HttpPost]
        public async Task<IActionResult> Cadastrar(IngressoCreateDto dto)
        {
            var resultado = await _ingressoService.Cadastrar(dto);

            if (resultado.erro != null)
                return BadRequest(new { mensagem = resultado.erro });

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = resultado.ingresso!.Id },
                resultado.ingresso
            );
        }

        // PUT: api/ingressos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            IngressoUpdateDto dto)
        {
            var resultado = await _ingressoService.Atualizar(id, dto);

            if (resultado.erro != null)
            {
                if (resultado.erro == "Ingresso não encontrado.")
                    return NotFound(new { mensagem = resultado.erro });

                return BadRequest(new { mensagem = resultado.erro });
            }

            return Ok(resultado.ingresso);
        }

        // DELETE: api/ingressos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await _ingressoService.Excluir(id);

            if (!excluido)
                return NotFound(new { mensagem = "Ingresso não encontrado." });

            return Ok(new { mensagem = "Ingresso cancelado com sucesso." });
        }
    }
}