namespace CinemaManagement.Models
{
    public class Assento
    {
        public int Id { get; set; }

        public int SalaId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        // Relacionamento
        public Sala Sala { get; set; } = null!;
    }
}
