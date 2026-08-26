using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }


        // ==========================================
        // GET - LISTAR TODOS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var clientes =
                await _clienteService.ListarTodos();

            return Ok(clientes);
        }


        // ==========================================
        // GET - BUSCAR POR ID
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var cliente =
                await _clienteService.BuscarPorId(id);

            if (cliente == null)
            {
                return NotFound(
                    new
                    {
                        mensagem =
                            "Cliente não encontrado."
                    }
                );
            }

            return Ok(cliente);
        }


        // ==========================================
        // POST - CADASTRAR
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Cadastrar(
            ClienteCreateDto dto)
        {
            try
            {
                var cliente =
                    await _clienteService.Cadastrar(dto);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = cliente.Id },
                    cliente
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        mensagem = ex.Message
                    }
                );
            }
        }


        // ==========================================
        // PUT - ATUALIZAR
        // ==========================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            ClienteUpdateDto dto)
        {
            try
            {
                var cliente =
                    await _clienteService.Atualizar(
                        id,
                        dto
                    );

                if (cliente == null)
                {
                    return NotFound(
                        new
                        {
                            mensagem =
                                "Cliente não encontrado."
                        }
                    );
                }

                return Ok(cliente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        mensagem = ex.Message
                    }
                );
            }
        }


        // ==========================================
        // DELETE
        // ==========================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido =
                await _clienteService.Excluir(id);

            if (!excluido)
            {
                return NotFound(
                    new
                    {
                        mensagem =
                            "Cliente não encontrado."
                    }
                );
            }

            return Ok(
                new
                {
                    mensagem =
                        "Cliente excluído com sucesso."
                }
            );
        }
    }
}