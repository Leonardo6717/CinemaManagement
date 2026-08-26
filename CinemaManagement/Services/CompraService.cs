using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class CompraService
    {
        private readonly CinemaDbContext _context;

        public CompraService(CinemaDbContext context)
        {
            _context = context;
        }

        // LISTAR TODAS AS COMPRAS ATIVAS
        public async Task<List<Compra>> ListarTodos()
        {
            return await _context.Compras
                .Include(c => c.Cliente)
                .Where(c => c.Ativa)
                .ToListAsync();
        }

        // BUSCAR COMPRA POR ID
        public async Task<Compra?> BuscarPorId(int id)
        {
            return await _context.Compras
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // CADASTRAR COMPRA
        public async Task<(Compra? compra, string? erro)> Cadastrar(
            CompraCreateDto dto)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

            if (cliente == null)
                return (null, "Cliente não encontrado.");

            if (!cliente.Ativo)
                return (null, "O cliente está inativo.");

            if (dto.ValorTotal <= 0)
                return (null, "O valor total deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(dto.FormaPagamento))
                return (null, "Informe a forma de pagamento.");

            var compra = new Compra
            {
                ClienteId = dto.ClienteId,
                ValorTotal = dto.ValorTotal,
                FormaPagamento = dto.FormaPagamento.Trim(),
                DataCompra = DateTime.Now,
                Status = "Concluida",
                Ativa = true
            };

            _context.Compras.Add(compra);

            await _context.SaveChangesAsync();

            return (compra, null);
        }

        // ATUALIZAR COMPRA
        public async Task<(Compra? compra, string? erro)> Atualizar(
            int id,
            CompraUpdateDto dto)
        {
            var compra = await _context.Compras
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null)
                return (null, "Compra não encontrada.");

            if (dto.ValorTotal <= 0)
                return (null, "O valor total deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(dto.FormaPagamento))
                return (null, "Informe a forma de pagamento.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return (null, "Informe o status da compra.");

            compra.ValorTotal = dto.ValorTotal;
            compra.FormaPagamento = dto.FormaPagamento.Trim();
            compra.Status = dto.Status.Trim();
            compra.Ativa = dto.Ativa;

            await _context.SaveChangesAsync();

            return (compra, null);
        }

        // CANCELAR / EXCLUSÃO LÓGICA
        public async Task<bool> Excluir(int id)
        {
            var compra = await _context.Compras
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null)
                return false;

            compra.Ativa = false;
            compra.Status = "Cancelada";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}