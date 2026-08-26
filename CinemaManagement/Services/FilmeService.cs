using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class FilmeService
    {
        private readonly CinemaDbContext _context;

        public FilmeService(CinemaDbContext context)
        {
            _context = context;
        }

        // LISTAR TODOS OS FILMES
        public async Task<List<Filme>> ListarTodos()
        {
            return await _context.Filmes
                .Where(f => f.Ativo)
                .ToListAsync();
        }

        // BUSCAR FILME POR ID
        public async Task<Filme?> BuscarPorId(int id)
        {
            return await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // CADASTRAR FILME
        public async Task<Filme> Cadastrar(FilmeCreateDto dto)
        {
            var filme = new Filme
            {
                Titulo = dto.Titulo,
                Sinopse = dto.Sinopse,
                Genero = dto.Genero,
                DuracaoMinutos = dto.DuracaoMinutos,
                ClassificacaoIndicativa = dto.ClassificacaoIndicativa,
                PosterUrl = dto.PosterUrl,
                DataEstreia = dto.DataEstreia,
                Ativo = true
            };

            _context.Filmes.Add(filme);

            await _context.SaveChangesAsync();

            return filme;
        }

        // ATUALIZAR FILME
        public async Task<Filme?> Atualizar(int id, FilmeUpdateDto dto)
        {
            var filme = await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == id);

            if (filme == null)
                return null;

            filme.Titulo = dto.Titulo;
            filme.Sinopse = dto.Sinopse;
            filme.Genero = dto.Genero;
            filme.DuracaoMinutos = dto.DuracaoMinutos;
            filme.ClassificacaoIndicativa = dto.ClassificacaoIndicativa;
            filme.PosterUrl = dto.PosterUrl;
            filme.DataEstreia = dto.DataEstreia;
            filme.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return filme;
        }

        // EXCLUIR FILME
        public async Task<bool> Excluir(int id)
        {
            var filme = await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == id);

            if (filme == null)
                return false;

            filme.Ativo = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
