using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessoesController : ControllerBase
    {
        private readonly SessaoService _sessaoService;

        public SessoesController(SessaoService sessaoService)
        {
            _sessaoService = sessaoService;
        }

        // GET: api/sessoes
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var sessoes = await _sessaoService.ListarTodas();

            return Ok(sessoes);
        }

        // GET: api/sessoes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var sessao = await _sessaoService.BuscarPorId(id);

            if (sessao == null)
                return NotFound(new { mensagem = "Sessão não encontrada." });

            return Ok(sessao);
        }

        // POST: api/sessoes
        [HttpPost]
        public async Task<IActionResult> Cadastrar(SessaoCreateDto dto)
        {
            var resultado = await _sessaoService.Cadastrar(dto);

            if (resultado.erro != null)
                return BadRequest(new { mensagem = resultado.erro });

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = resultado.sessao!.Id },
                resultado.sessao
            );
        }

        // PUT: api/sessoes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            SessaoUpdateDto dto)
        {
            var resultado = await _sessaoService.Atualizar(id, dto);

            if (resultado.erro != null)
            {
                if (resultado.erro == "Sessão não encontrada.")
                    return NotFound(new { mensagem = resultado.erro });

                return BadRequest(new { mensagem = resultado.erro });
            }

            return Ok(resultado.sessao);
        }

        // DELETE: api/sessoes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluida = await _sessaoService.Excluir(id);

            if (!excluida)
                return NotFound(new { mensagem = "Sessão não encontrada." });

            return Ok(new { mensagem = "Sessão excluída com sucesso." });
        }
    }
}