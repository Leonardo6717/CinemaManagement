using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class SalaService
    {
        private readonly CinemaDbContext _context;

        public SalaService(CinemaDbContext context)
        {
            _context = context;
        }

        // LISTAR TODAS AS SALAS ATIVAS
        public async Task<List<Sala>> ListarTodas()
        {
            return await _context.Salas
                .Where(s => s.Ativa)
                .ToListAsync();
        }

        // BUSCAR SALA POR ID
        public async Task<Sala?> BuscarPorId(int id)
        {
            return await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // CADASTRAR SALA
        public async Task<Sala> Cadastrar(SalaCreateDto dto)
        {
            var sala = new Sala
            {
                Nome = dto.Nome,
                Capacidade = dto.Capacidade,
                Tipo = dto.Tipo,
                Ativa = true
            };

            _context.Salas.Add(sala);

            await _context.SaveChangesAsync();

            return sala;
        }

        // ATUALIZAR SALA
        public async Task<Sala?> Atualizar(int id, SalaUpdateDto dto)
        {
            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sala == null)
                return null;

            sala.Nome = dto.Nome;
            sala.Capacidade = dto.Capacidade;
            sala.Tipo = dto.Tipo;
            sala.Ativa = dto.Ativa;

            await _context.SaveChangesAsync();

            return sala;
        }

        // EXCLUSÃO LÓGICA
        public async Task<bool> Excluir(int id)
        {
            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sala == null)
                return false;

            sala.Ativa = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
