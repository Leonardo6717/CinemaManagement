namespace CinemaManagement.Models
{
    public class Sessao
    {
        public int Id { get; set; }

        public int FilmeId { get; set; }

        public int SalaId { get; set; }

        public DateTime DataHora { get; set; }

        public decimal PrecoIngresso { get; set; }

        public bool Ativa { get; set; } = true;

        // Relacionamentos
        public Filme Filme { get; set; } = null!;

        public Sala Sala { get; set; } = null!;
    }
}
