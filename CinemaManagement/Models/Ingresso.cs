namespace CinemaManagement.Models
{
    public class Ingresso
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int SessaoId { get; set; }

        public string Assento { get; set; } = string.Empty;

        public decimal PrecoPago { get; set; }

        public DateTime DataCompra { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public Cliente Cliente { get; set; } = null!;

        public Sessao Sessao { get; set; } = null!;
    }
}
