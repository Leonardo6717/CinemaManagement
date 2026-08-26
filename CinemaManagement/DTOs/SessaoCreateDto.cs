namespace CinemaManagement.DTOs
{
    public class SessaoCreateDto
    {
        public int FilmeId { get; set; }

        public int SalaId { get; set; }

        public DateTime DataHora { get; set; }

        public decimal PrecoIngresso { get; set; }
    }
}
