
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace CinemaManagement.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
        {
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ingresso> Ingressos { get; set; }
        public DbSet<Assento> Assentos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Compra> Compras { get; set; }

    }
}
