using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class AssentoService
    {
        private readonly CinemaDbContext _context;

        public AssentoService(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assento>> ListarPorSala(int salaId)
        {
            return await _context.Assentos
                .Where(a => a.SalaId == salaId && a.Ativo)
                .OrderBy(a => a.Codigo)
                .ToListAsync();
        }

        public async Task<(List<Assento>? assentos, string? erro)> GerarAssentos(
            AssentoGerarDto dto)
        {
            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == dto.SalaId);

            if (sala == null)
                return (null, "Sala não encontrada.");

            if (!sala.Ativa)
                return (null, "A sala está inativa.");

            if (dto.QuantidadeFileiras <= 0 || dto.AssentosPorFileira <= 0)
                return (null, "Quantidade de fileiras e assentos deve ser maior que zero.");

            int total = dto.QuantidadeFileiras * dto.AssentosPorFileira;

            if (total > sala.Capacidade)
                return (null, "A quantidade de assentos ultrapassa a capacidade da sala.");

            var jaExistem = await _context.Assentos
                .AnyAsync(a => a.SalaId == dto.SalaId && a.Ativo);

            if (jaExistem)
                return (null, "Essa sala já possui assentos cadastrados.");

            var assentos = new List<Assento>();

            for (int fileira = 0; fileira < dto.QuantidadeFileiras; fileira++)
            {
                char letra = (char)('A' + fileira);

                for (int numero = 1; numero <= dto.AssentosPorFileira; numero++)
                {
                    assentos.Add(new Assento
                    {
                        SalaId = dto.SalaId,
                        Codigo = $"{letra}{numero}",
                        Ativo = true
                    });
                }
            }

            _context.Assentos.AddRange(assentos);

            await _context.SaveChangesAsync();

            return (assentos, null);
        }

        public async Task<bool> Excluir(int id)
        {
            var assento = await _context.Assentos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assento == null)
                return false;

            assento.Ativo = false;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<(List<AssentoSessaoDto>? assentos, string? erro)>
    ListarPorSessao(int sessaoId)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(s => s.Id == sessaoId);

            if (sessao == null)
                return (null, "Sessão não encontrada.");

            var assentos = await _context.Assentos
                .Where(a => a.SalaId == sessao.SalaId && a.Ativo)
                .OrderBy(a => a.Codigo)
                .ToListAsync();

            var ingressosVendidos = await _context.Ingressos
                .Where(i => i.SessaoId == sessaoId && i.Ativo)
                .Select(i => i.Assento)
                .ToListAsync();

            var resultado = assentos.Select(a => new AssentoSessaoDto
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Ocupado = ingressosVendidos.Contains(a.Codigo)
            }).ToList();

            return (resultado, null);
        }
    }
}
