using CinemaManagement.DTOs;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaService _reservaService;

        public ReservasController(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/reservas
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var reservas = await _reservaService.ListarTodas();

            return Ok(reservas);
        }

        // GET: api/reservas/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var reserva = await _reservaService.BuscarPorId(id);

            if (reserva == null)
                return NotFound(new
                {
                    mensagem = "Reserva não encontrada."
                });

            return Ok(reserva);
        }

        // POST: api/reservas
        [HttpPost]
        public async Task<IActionResult> Cadastrar(ReservaCreateDto dto)
        {
            var resultado = await _reservaService.Cadastrar(dto);

            if (resultado.erro != null)
            {
                return BadRequest(new
                {
                    mensagem = resultado.erro
                });
            }

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = resultado.reserva!.Id },
                resultado.reserva
            );
        }

        // PUT: api/reservas/1/cancelar
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var cancelada = await _reservaService.Cancelar(id);

            if (!cancelada)
            {
                return NotFound(new
                {
                    mensagem = "Reserva não encontrada."
                });
            }

            return Ok(new
            {
                mensagem = "Reserva cancelada com sucesso."
            });
        }
    }
}
