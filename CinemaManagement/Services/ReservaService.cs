using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ReservaService
    {
        private readonly CinemaDbContext _context;

        public ReservaService(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ListarTodas()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Filme)
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Sala)
                .Include(r => r.Assento)
                .ToListAsync();
        }

        public async Task<Reserva?> BuscarPorId(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Filme)
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Sala)
                .Include(r => r.Assento)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(Reserva? reserva, string? erro)> Cadastrar(
            ReservaCreateDto dto)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

            if (cliente == null)
                return (null, "Cliente não encontrado.");

            if (!cliente.Ativo)
                return (null, "O cliente está inativo.");

            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(s => s.Id == dto.SessaoId);

            if (sessao == null)
                return (null, "Sessão não encontrada.");

            if (!sessao.Ativa)
                return (null, "A sessão está inativa.");

            var assento = await _context.Assentos
                .FirstOrDefaultAsync(a => a.Id == dto.AssentoId);

            if (assento == null)
                return (null, "Assento não encontrado.");

            if (!assento.Ativo)
                return (null, "O assento está inativo.");

            if (assento.SalaId != sessao.SalaId)
                return (null, "O assento não pertence à sala desta sessão.");

            var ingressoJaExiste = await _context.Ingressos
                .AnyAsync(i =>
                    i.SessaoId == dto.SessaoId &&
                    i.Assento == assento.Codigo &&
                    i.Ativo);

            if (ingressoJaExiste)
                return (null, "Este assento já foi vendido.");

            var reservaAtiva = await _context.Reservas
                .AnyAsync(r =>
                    r.SessaoId == dto.SessaoId &&
                    r.AssentoId == dto.AssentoId &&
                    r.Status == "Pendente" &&
                    r.ExpiraEm > DateTime.Now);

            if (reservaAtiva)
                return (null, "Este assento já está reservado.");

            var reserva = new Reserva
            {
                ClienteId = dto.ClienteId,
                SessaoId = dto.SessaoId,
                AssentoId = dto.AssentoId,
                DataReserva = DateTime.Now,
                ExpiraEm = DateTime.Now.AddMinutes(10),
                Status = "Pendente"
            };

            _context.Reservas.Add(reserva);

            await _context.SaveChangesAsync();

            return (reserva, null);
        }

        public async Task<bool> Cancelar(int id)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return false;

            reserva.Status = "Cancelada";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
