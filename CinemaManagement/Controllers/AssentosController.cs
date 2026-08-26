using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssentosController : ControllerBase
    {
        private readonly AssentoService _assentoService;

        public AssentosController(AssentoService assentoService)
        {
            _assentoService = assentoService;
        }

        // GET: api/assentos/sala/1
        [HttpGet("sala/{salaId}")]
        public async Task<IActionResult> ListarPorSala(int salaId)
        {
            var assentos = await _assentoService.ListarPorSala(salaId);

            return Ok(assentos);
        }
        // GET: api/assentos/sessao/1
        [HttpGet("sessao/{sessaoId}")]
        public async Task<IActionResult> ListarPorSessao(int sessaoId)
        {
            var resultado = await _assentoService.ListarPorSessao(sessaoId);

            if (resultado.erro != null)
                return NotFound(new { mensagem = resultado.erro });

            return Ok(resultado.assentos);
        }

        // POST: api/assentos/gerar
        [HttpPost("gerar")]
        public async Task<IActionResult> GerarAssentos(AssentoGerarDto dto)
        {
            var resultado = await _assentoService.GerarAssentos(dto);

            if (resultado.erro != null)
                return BadRequest(new { mensagem = resultado.erro });

            return Ok(new
            {
                mensagem = "Assentos gerados com sucesso.",
                quantidade = resultado.assentos!.Count,
                assentos = resultado.assentos
            });
        }

        // DELETE: api/assentos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await _assentoService.Excluir(id);

            if (!excluido)
                return NotFound(new { mensagem = "Assento não encontrado." });

            return Ok(new { mensagem = "Assento desativado com sucesso." });
        }
    }
}