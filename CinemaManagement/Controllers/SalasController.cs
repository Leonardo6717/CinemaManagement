using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalasController : ControllerBase
    {
        private readonly SalaService _salaService;

        public SalasController(SalaService salaService)
        {
            _salaService = salaService;
        }

        // GET: api/salas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var salas = await _salaService.ListarTodas();

            return Ok(salas);
        }

        // GET: api/salas/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var sala = await _salaService.BuscarPorId(id);

            if (sala == null)
                return NotFound(new { mensagem = "Sala não encontrada." });

            return Ok(sala);
        }

        // POST: api/salas
        [HttpPost]
        public async Task<IActionResult> Cadastrar(SalaCreateDto dto)
        {
            var sala = await _salaService.Cadastrar(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = sala.Id },
                sala
            );
        }

        // PUT: api/salas/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            SalaUpdateDto dto)
        {
            var sala = await _salaService.Atualizar(id, dto);

            if (sala == null)
                return NotFound(new { mensagem = "Sala não encontrada." });

            return Ok(sala);
        }

        // DELETE: api/salas/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluida = await _salaService.Excluir(id);

            if (!excluida)
                return NotFound(new { mensagem = "Sala não encontrada." });

            return Ok(new { mensagem = "Sala excluída com sucesso." });
        }
    }
}
