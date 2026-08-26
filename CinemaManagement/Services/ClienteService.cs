using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ClienteService
    {
        private readonly CinemaDbContext _context;

        public ClienteService(CinemaDbContext context)
        {
            _context = context;
        }


        // LISTAR CLIENTES ATIVOS
        public async Task<List<Cliente>> ListarTodos()
        {
            return await _context.Clientes
                .Where(c => c.Ativo)
                .ToListAsync();
        }


        // BUSCAR CLIENTE POR ID
        public async Task<Cliente?> BuscarPorId(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        // CADASTRAR CLIENTE
        public async Task<Cliente> Cadastrar(
            ClienteCreateDto dto)
        {
            if (dto.DataNascimento.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data de nascimento não pode ser futura."
                );
            }


            var cliente = new Cliente
            {
                Nome = dto.Nome.Trim(),

                Email = dto.Email
                    .Trim()
                    .ToLower(),

                Telefone = dto.Telefone.Trim(),

                DataNascimento =
                    dto.DataNascimento.Date,

                Ativo = true
            };


            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return cliente;
        }


        // ATUALIZAR CLIENTE
        public async Task<Cliente?> Atualizar(
            int id,
            ClienteUpdateDto dto)
        {
            var cliente =
                await _context.Clientes
                    .FirstOrDefaultAsync(
                        c => c.Id == id
                    );


            if (cliente == null)
                return null;


            if (dto.DataNascimento.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data de nascimento não pode ser futura."
                );
            }


            cliente.Nome =
                dto.Nome.Trim();

            cliente.Email =
                dto.Email
                    .Trim()
                    .ToLower();

            cliente.Telefone =
                dto.Telefone.Trim();

            cliente.DataNascimento =
                dto.DataNascimento.Date;

            cliente.Ativo =
                dto.Ativo;


            await _context.SaveChangesAsync();

            return cliente;
        }


        // EXCLUSÃO LÓGICA
        public async Task<bool> Excluir(int id)
        {
            var cliente =
                await _context.Clientes
                    .FirstOrDefaultAsync(
                        c => c.Id == id
                    );


            if (cliente == null)
                return false;


            cliente.Ativo = false;


            await _context.SaveChangesAsync();

            return true;
        }
    }
}