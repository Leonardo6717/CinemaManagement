using CinemaManagement.Data;
using CinemaManagement.DTOs;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class IngressoService
    {
        private readonly CinemaDbContext _context;

        public IngressoService(CinemaDbContext context)
        {
            _context = context;
        }


        // ======================================================
        // LISTAR TODOS OS INGRESSOS ATIVOS
        // ======================================================

        public async Task<List<Ingresso>> ListarTodos()
        {
            return await _context.Ingressos

                .Include(i => i.Cliente)

                .Include(i => i.Sessao)
                    .ThenInclude(s => s.Filme)

                .Include(i => i.Sessao)
                    .ThenInclude(s => s.Sala)

                .Where(i => i.Ativo)

                .ToListAsync();
        }


        // ======================================================
        // BUSCAR INGRESSO POR ID
        // ======================================================

        public async Task<Ingresso?> BuscarPorId(int id)
        {
            return await _context.Ingressos

                .Include(i => i.Cliente)

                .Include(i => i.Sessao)
                    .ThenInclude(s => s.Filme)

                .Include(i => i.Sessao)
                    .ThenInclude(s => s.Sala)

                .FirstOrDefaultAsync(
                    i => i.Id == id
                );
        }


        // ======================================================
        // CADASTRAR / VENDER INGRESSO
        // ======================================================

        public async Task<(Ingresso? ingresso, string? erro)> Cadastrar(
            IngressoCreateDto dto)
        {
            // ==================================================
            // VERIFICAR CLIENTE
            // ==================================================

            var cliente =
                await _context.Clientes
                    .FirstOrDefaultAsync(
                        c => c.Id == dto.ClienteId
                    );


            if (cliente == null)
            {
                return (
                    null,
                    "Cliente não encontrado."
                );
            }


            if (!cliente.Ativo)
            {
                return (
                    null,
                    "O cliente está inativo."
                );
            }


            // ==================================================
            // VERIFICAR SESSÃO
            // ==================================================

            var sessao =
                await _context.Sessoes
                    .FirstOrDefaultAsync(
                        s => s.Id == dto.SessaoId
                    );


            if (sessao == null)
            {
                return (
                    null,
                    "Sessão não encontrada."
                );
            }


            if (!sessao.Ativa)
            {
                return (
                    null,
                    "A sessão está inativa."
                );
            }


            // ==================================================
            // PADRONIZAR ASSENTO
            // ==================================================

            var codigoAssento =
                dto.Assento
                    .Trim()
                    .ToUpper();


            if (string.IsNullOrWhiteSpace(codigoAssento))
            {
                return (
                    null,
                    "Informe um assento."
                );
            }


            // ==================================================
            // VERIFICAR SE O ASSENTO EXISTE NA SALA
            // ==================================================

            var assentoExiste =
                await _context.Assentos
                    .AnyAsync(
                        a =>
                            a.SalaId == sessao.SalaId &&
                            a.Codigo == codigoAssento &&
                            a.Ativo
                    );


            if (!assentoExiste)
            {
                return (
                    null,
                    "O assento informado não existe nesta sala."
                );
            }


            // ==================================================
            // VERIFICAR SE O ASSENTO JÁ FOI VENDIDO
            // ==================================================

            var assentoOcupado =
                await _context.Ingressos
                    .AnyAsync(
                        i =>
                            i.SessaoId == dto.SessaoId &&
                            i.Assento == codigoAssento &&
                            i.Ativo
                    );


            if (assentoOcupado)
            {
                return (
                    null,
                    "Este assento já foi vendido para essa sessão."
                );
            }


            // ==================================================
            // CRIAR INGRESSO
            // ==================================================

            var ingresso =
                new Ingresso
                {
                    ClienteId =
                        dto.ClienteId,

                    SessaoId =
                        dto.SessaoId,

                    Assento =
                        codigoAssento,

                    // IMPORTANTE:
                    // O preço vem do banco/sessão.
                    // Não confiamos no valor enviado pelo usuário.
                    PrecoPago =
                        sessao.PrecoIngresso,

                    DataCompra =
                        DateTime.Now,

                    Ativo =
                        true
                };


            _context.Ingressos.Add(
                ingresso
            );


            await _context.SaveChangesAsync();


            return (
                ingresso,
                null
            );
        }


        // ======================================================
        // ATUALIZAR INGRESSO
        // ======================================================

        public async Task<(Ingresso? ingresso, string? erro)> Atualizar(
            int id,
            IngressoUpdateDto dto)
        {
            // ==================================================
            // VERIFICAR INGRESSO
            // ==================================================

            var ingresso =
                await _context.Ingressos
                    .FirstOrDefaultAsync(
                        i => i.Id == id
                    );


            if (ingresso == null)
            {
                return (
                    null,
                    "Ingresso não encontrado."
                );
            }


            // ==================================================
            // VERIFICAR CLIENTE
            // ==================================================

            var cliente =
                await _context.Clientes
                    .FirstOrDefaultAsync(
                        c => c.Id == dto.ClienteId
                    );


            if (cliente == null)
            {
                return (
                    null,
                    "Cliente não encontrado."
                );
            }


            if (!cliente.Ativo)
            {
                return (
                    null,
                    "O cliente está inativo."
                );
            }


            // ==================================================
            // VERIFICAR SESSÃO
            // ==================================================

            var sessao =
                await _context.Sessoes
                    .FirstOrDefaultAsync(
                        s => s.Id == dto.SessaoId
                    );


            if (sessao == null)
            {
                return (
                    null,
                    "Sessão não encontrada."
                );
            }


            if (!sessao.Ativa)
            {
                return (
                    null,
                    "A sessão está inativa."
                );
            }


            // ==================================================
            // PADRONIZAR ASSENTO
            // ==================================================

            var codigoAssento =
                dto.Assento
                    .Trim()
                    .ToUpper();


            if (string.IsNullOrWhiteSpace(codigoAssento))
            {
                return (
                    null,
                    "Informe um assento."
                );
            }


            // ==================================================
            // VERIFICAR SE ASSENTO EXISTE
            // ==================================================

            var assentoExiste =
                await _context.Assentos
                    .AnyAsync(
                        a =>
                            a.SalaId == sessao.SalaId &&
                            a.Codigo == codigoAssento &&
                            a.Ativo
                    );


            if (!assentoExiste)
            {
                return (
                    null,
                    "O assento informado não existe nesta sala."
                );
            }


            // ==================================================
            // VERIFICAR SE OUTRO INGRESSO JÁ OCUPA O ASSENTO
            // ==================================================

            var assentoOcupado =
                await _context.Ingressos
                    .AnyAsync(
                        i =>
                            i.Id != id &&
                            i.SessaoId == dto.SessaoId &&
                            i.Assento == codigoAssento &&
                            i.Ativo
                    );


            if (assentoOcupado)
            {
                return (
                    null,
                    "Este assento já foi vendido para essa sessão."
                );
            }


            // ==================================================
            // ATUALIZAR INGRESSO
            // ==================================================

            ingresso.ClienteId =
                dto.ClienteId;

            ingresso.SessaoId =
                dto.SessaoId;

            ingresso.Assento =
                codigoAssento;

            // Também usamos o preço real da sessão no PUT.
            ingresso.PrecoPago =
                sessao.PrecoIngresso;

            ingresso.Ativo =
                dto.Ativo;


            await _context.SaveChangesAsync();


            return (
                ingresso,
                null
            );
        }


        // ======================================================
        // CANCELAR / EXCLUSÃO LÓGICA
        // ======================================================

        public async Task<bool> Excluir(int id)
        {
            var ingresso =
                await _context.Ingressos
                    .FirstOrDefaultAsync(
                        i => i.Id == id
                    );


            if (ingresso == null)
            {
                return false;
            }


            ingresso.Ativo =
                false;


            await _context.SaveChangesAsync();


            return true;
        }
    }
}