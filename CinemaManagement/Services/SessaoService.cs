using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class SessaoService
    {
        private readonly CinemaDbContext _context;

        public SessaoService(CinemaDbContext context)
        {
            _context = context;
        }

        // LISTAR TODAS AS SESSÕES ATIVAS
        public async Task<List<Sessao>> ListarTodas()
        {
            return await _context.Sessoes
                .Include(s => s.Filme)
                .Include(s => s.Sala)
                .Where(s => s.Ativa)
                .ToListAsync();
        }

        // BUSCAR SESSÃO POR ID
        public async Task<Sessao?> BuscarPorId(int id)
        {
            return await _context.Sessoes
                .Include(s => s.Filme)
                .Include(s => s.Sala)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // CADASTRAR SESSÃO
        public async Task<(Sessao? sessao, string? erro)> Cadastrar(SessaoCreateDto dto)
        {
            var filme = await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == dto.FilmeId);

            if (filme == null)
                return (null, "Filme não encontrado.");

            if (!filme.Ativo)
                return (null, "O filme informado está inativo.");

            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == dto.SalaId);

            if (sala == null)
                return (null, "Sala não encontrada.");

            if (!sala.Ativa)
                return (null, "A sala informada está inativa.");

            var sessao = new Sessao
            {
                FilmeId = dto.FilmeId,
                SalaId = dto.SalaId,
                DataHora = dto.DataHora,
                PrecoIngresso = dto.PrecoIngresso,
                Ativa = true
            };

            _context.Sessoes.Add(sessao);

            await _context.SaveChangesAsync();

            return (sessao, null);
        }

        // ATUALIZAR SESSÃO
        public async Task<(Sessao? sessao, string? erro)> Atualizar(
            int id,
            SessaoUpdateDto dto)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sessao == null)
                return (null, "Sessão não encontrada.");

            var filme = await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == dto.FilmeId);

            if (filme == null)
                return (null, "Filme não encontrado.");

            if (!filme.Ativo)
                return (null, "O filme informado está inativo.");

            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == dto.SalaId);

            if (sala == null)
                return (null, "Sala não encontrada.");

            if (!sala.Ativa)
                return (null, "A sala informada está inativa.");

            sessao.FilmeId = dto.FilmeId;
            sessao.SalaId = dto.SalaId;
            sessao.DataHora = dto.DataHora;
            sessao.PrecoIngresso = dto.PrecoIngresso;
            sessao.Ativa = dto.Ativa;

            await _context.SaveChangesAsync();

            return (sessao, null);
        }

        // EXCLUSÃO LÓGICA
        public async Task<bool> Excluir(int id)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sessao == null)
                return false;

            sessao.Ativa = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}