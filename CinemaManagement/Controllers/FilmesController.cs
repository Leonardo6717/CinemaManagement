using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmesController : ControllerBase
    {
        private readonly FilmeService _filmeService;

        public FilmesController(FilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        // GET: api/filmes
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var filmes = await _filmeService.ListarTodos();

            return Ok(filmes);
        }

        // GET: api/filmes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var filme = await _filmeService.BuscarPorId(id);

            if (filme == null)
                return NotFound(new { mensagem = "Filme não encontrado." });

            return Ok(filme);
        }

        // POST: api/filmes
        [HttpPost]
        public async Task<IActionResult> Cadastrar(FilmeCreateDto dto)
        {
            var filme = await _filmeService.Cadastrar(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = filme.Id },
                filme
            );
        }

        // PUT: api/filmes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            FilmeUpdateDto dto)
        {
            var filme = await _filmeService.Atualizar(id, dto);

            if (filme == null)
                return NotFound(new { mensagem = "Filme não encontrado." });

            return Ok(filme);
        }

        // DELETE: api/filmes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await _filmeService.Excluir(id);

            if (!excluido)
                return NotFound(new { mensagem = "Filme não encontrado." });

            return Ok(new { mensagem = "Filme excluído com sucesso." });
        }
    }
}